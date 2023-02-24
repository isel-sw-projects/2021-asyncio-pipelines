package AsyncFile;

import io.reactivex.rxjava3.core.Observable;
import io.reactivex.rxjava3.core.ObservableEmitter;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.io.UnsupportedEncodingException;
import java.nio.ByteBuffer;
import java.nio.channels.AsynchronousFileChannel;
import java.nio.channels.CompletionHandler;
import java.nio.file.Path;
import java.nio.file.StandardOpenOption;
import java.security.InvalidParameterException;
import java.util.concurrent.CompletableFuture;
import java.util.function.BiConsumer;

/**
 * @author Jorge Martins
 */
public class AsyncFile {
    // tells if file is open for read or open for write
    private enum Mode { Read, Write}

    // the transfer buffer size
    // A problem with too many recursive calls remain
    // we must check this and force the continuation in a new thread
    // for now we reduce the buffer size :(
    private static final int CHUNKSIZE = 4096*8;
    //
    // Stuff needed for asynchronous readline
    //
    private static final int BUFFER_SIZE= CHUNKSIZE;
    private static final int MAX_LINE_SIZE = 4096;
    private static final int LF = '\n';
    private static final int CR = '\r';

    // the transfer buffer
    private byte[] auxline = new byte[MAX_LINE_SIZE];

    // buffer for current producing line
    private byte[] buffer = new byte[BUFFER_SIZE];

    // read position in buffer
    int bufpos = 0;

    // total bytes in buffer
    int bufsize=0;

    // current position in producing line
    int linepos=0;

    // the nio associated file channel
    private AsynchronousFileChannel channel;

    // read or write
    private Mode mode;

    // current read or write position in file
    private long position;

    /**
     * private constructor just for internal use
     */
    private AsyncFile(Path path, Mode mode) throws IOException {
        if (mode == Mode.Read )
            channel = AsynchronousFileChannel.open(path, StandardOpenOption.READ);
        else
            channel = AsynchronousFileChannel.open(path,
                    StandardOpenOption.CREATE,
                    StandardOpenOption.WRITE,
                    StandardOpenOption.TRUNCATE_EXISTING
            );
    }

    public static Observable<String> lines2(Path path) {
        AsyncFile file = open(path);
        return Observable
                .create(source -> file.emmitLines(source));
    }

    public static CompletableFuture<Void> readAllLines(Path path, BiConsumer<String, Throwable> cb) {
        AsyncFile file = open(path);
        return file
                .readAllLines(cb)
                .whenComplete((err, nothing) ->  file.close());
    }

    /**
     * Asynchronous line read operation through callback.
     * On each branch this readline() either invokes the callback or completes the CF finish.
     */
    private CompletableFuture<Void> readAllLines(BiConsumer<String, Throwable> cb) {
        CompletableFuture<Void> finish = new CompletableFuture<>();
        readAllLines(cb, finish);
        return finish;
    }

    // file factory for read mode
    public static AsyncFile open(Path file) {
        try {
            return new AsyncFile(file, Mode.Read);
        }
        catch(IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    /**
     * Asynchronous line read operation through callback.
     * On each branch this readline() either invokes the callback or completes the CF finish.
     */
    private void readAllLines(BiConsumer<String, Throwable> cb, CompletableFuture<Void> finish) {
        while(bufpos < bufsize) {
            if (buffer[bufpos] == LF) {
                if (linepos > 0 && auxline[linepos-1] == CR) linepos--;
                bufpos++;
                cb.accept(produceLine(), null);
            }
            else if (linepos == MAX_LINE_SIZE -1) cb.accept(produceLine(), null);
            else auxline[linepos++] = buffer[bufpos++];
        }

        readBytes(buffer, 0, buffer.length, (err, res) -> {
            if(err != null) {
                cb.accept(null, err);
                finish.completeExceptionally(err);
                return;
            }
            if (res <= 0) {
                bufpos = 0; bufsize=0;
                // needed for last line that doesn't end with LF
                if (linepos > 0) {
                    cb.accept(produceLine(), null);
                }
                finish.complete(null);
                return;
            }
            bufsize=res; bufpos = 0;
            readAllLines(cb, finish);
        });
    }
    private String produceLine() {
        String l ;
        try {
            l= new String(auxline, 0, linepos, "UTF-8");
        }
        catch(UnsupportedEncodingException e) {
            l= null;
        }
        linepos = 0;
        return l;
    }


    // asynchronous line read operation returning a CompletableFuture<String>
    public void  emmitLines(ObservableEmitter<String> source) {

        while(bufpos < bufsize) {
            if (buffer[bufpos] == LF) {
                if (linepos > 0 && auxline[linepos-1] == CR) linepos--;
                bufpos++;
                String line = produceLine();
                //System.out.println(line);
                source.onNext(line);
            }
            else if (linepos == MAX_LINE_SIZE -1) {
                String line = produceLine();
                //System.out.println(line);
                source.onNext(line);
            }
            else auxline[linepos++] = buffer[bufpos++];
        }
        readBytes(buffer)
                .exceptionally(err-> {source.onError(err); return 0; })
                .thenAccept(res -> {
                    if (res <= 0) {
                        bufsize=0;
                        // needed for last line that doesn't end with LF
                        if (linepos > 0) {
                            String line = produceLine();
                            //System.out.println(line);
                            source.onNext(line);
                        }
                        source.onComplete();
                        return;
                    }
                    bufpos = 0; bufsize=res;
                    emmitLines(source);
                });

    }

    // asynchronous read buffer operation returning a CompletableFuture
    public CompletableFuture<Integer> readBytes(byte[] data) {
        return readBytes(data,0, data.length);
    }

    // asynchronous read chunk operation returning a CompletableFuture
    public CompletableFuture<Integer> readBytes(byte[] data, int ofs, int size) {
        CompletableFuture<Integer> completed = new CompletableFuture<>();

        readBytes(data, ofs, size,
                (t,i) -> {
                    if (t== null) completed.complete(i);
                    else completed.completeExceptionally(t);
                });
        return completed;
    }

    // asynchronous read chunk operation, callback based
    public void readBytes(byte[] data, int ofs, int size,
                          BiConsumer<Throwable, Integer> completed) {
        if (completed == null)
            throw new InvalidParameterException("callback can't be null!");
        if (mode == Mode.Write)
            throw new IllegalStateException("File is not readable");
        if (size + ofs > data.length)
            size = data.length - ofs;
        if (size ==0) {
            completed.accept(null, 0);
            return;
        }
        int s = size;
        ByteBuffer buf = ByteBuffer.wrap(data, ofs, size);
        CompletionHandler<Integer,Object> readCompleted =
                new CompletionHandler<Integer,Object>() {

                    @Override
                    public void completed(Integer result, Object attachment) {
                        if (result>0) position += result;
                        completed.accept(null,result);
                    }

                    @Override
                    public void failed(Throwable exc, Object attachment) {

                        completed.accept(exc, null);
                    }
                };
        channel.read(buf, position, null, readCompleted);
    }
    public void close() {
        try {
            channel.close();
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }
}

