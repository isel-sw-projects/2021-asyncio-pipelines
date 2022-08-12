package lookwords.FindBiggestWordStrategies;

import org.javaync.io.AsyncFiles;
import org.reactivestreams.Subscriber;
import org.reactivestreams.Subscription;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.BiConsumer;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public class FindWordsRxIo implements FindBiggestWord {
    Containner<String> cont = new Containner<>("");
    Object mon = new Object();
    public final Containner<String> findBiggestWord(String folder) {

        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            paths
                .filter(Files::isRegularFile)
                .map(path -> readInto(path))
                .collect(toList())
                .forEach(CompletableFuture::join);
            return cont;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    private final CompletableFuture<Void> readInto(Path file) {
        CompletableFuture<Void> cf = new CompletableFuture<>();
        class GutenbergLinesParser {
            int count = 0;
            boolean finish = false;
            public void parse(String line, Throwable err) {
                if(finish) return;
                if(err != null) {
                    finish = true;
                    cf.completeExceptionally(err);
                    return;
                }
                if (line == null || line.contains("*** END OF ")) {
                    finish = true;
                    cf.complete(null);
                    return;
                }
                if(!line.isEmpty()) {
                    if(count > 14) {
                        for(String w : line.split(" ")) {
                            synchronized (mon) {
                                if (w.length() > cont.value.length()) {
                                    cont.value = w;
                                }
                            }
                        }
                    } else {
                        count++;
                    }
                }
            }
        }
        readInto(file, new GutenbergLinesParser()::parse);
        return cf;
    }

    /**
     * !!!! MISSING cancel Subscription !!!!
     */
    private static void readInto(Path file, BiConsumer<String, Throwable> parser) {
        AsyncFiles
            .lines(file)
            .subscribe(new Subscriber<String>() {
                public void onSubscribe(Subscription s) { s.request(Integer.MAX_VALUE);}
                public void onError(Throwable t) { parser.accept(null, t); }
                public void onComplete() { parser.accept(null, null); }
                public void onNext(String line) { parser.accept(line, null);}
            });
    }

}
