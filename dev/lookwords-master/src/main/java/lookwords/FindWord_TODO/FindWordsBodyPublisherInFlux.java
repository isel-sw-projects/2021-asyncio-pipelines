package lookwords.FindWord_TODO;

import reactor.core.publisher.Flux;

import java.io.FileNotFoundException;
import java.io.UncheckedIOException;
import java.net.http.HttpRequest;
import java.nio.ByteBuffer;
import java.nio.file.Path;
import java.util.concurrent.Flow;

import static org.reactivestreams.FlowAdapters.toPublisher;
import static reactor.core.publisher.Flux.fromArray;

public class FindWordsBodyPublisherInFlux {


    protected Flux<String> lines(Path file, Containner<String> cont) {
        Object mon = new Object();
        ByteBufferToLines lines = new ByteBufferToLines();
        return Flux
            .from(toPublisher(ofFile(file)))
            .flatMap(lines::flux) // !!! Not Scale even with: .map(buffer -> new String(buffer.array()))
            .filter(line -> !line.isEmpty())                   // Skip empty lines
            .skip(14)                                          // Skip gutenberg header
            .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
            .flatMap(line -> fromArray(line.split(" ")))
                .doOnNext(w -> {
                    synchronized (mon) {
                        if (cont.value.length() < w.length()) {
                            cont.value = w;
                        }
                    }
                });
    }

    private static class ByteBufferToLines {

        private final StringBuilder prev = new StringBuilder();

        Flux<String> flux(ByteBuffer chunk) {
            prev.append(new String(chunk.array())); // join previous with current
            if(prev.indexOf("\n") < 0)              // return empty if there are no lines delimiters
                return Flux.empty();
            String[] lines = prev.toString().split("\n|(\r\n)");     // split in lines
            prev.setLength(0);                                       // clear the builder
            if(lines.length > 1) prev.append(lines[lines.length-1]); // append the last statement
            return lines.length > 1
                ? fromArray(lines).take(lines.length - 1L) // drop last statement
                : fromArray(lines);
        }
    }

    private static Flow.Publisher<ByteBuffer> ofFile(Path file) {
        try {
            return HttpRequest.BodyPublishers.ofFile(file);
        } catch (FileNotFoundException e) {
            throw new UncheckedIOException(e);
        }
    }
}
