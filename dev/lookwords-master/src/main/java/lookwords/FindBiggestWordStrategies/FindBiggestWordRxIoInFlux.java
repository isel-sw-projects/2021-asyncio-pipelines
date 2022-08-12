package lookwords.FindBiggestWordStrategies;

import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;

import java.nio.file.Path;

import static reactor.core.publisher.Flux.fromArray;

public class FindBiggestWordRxIoInFlux {
    Object mon = new Object();

    protected Flux<String> lines(Path file, Containner<String> cont) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Flux
            .from(lines)
            .filter(line -> !line.isEmpty())                   // Skip empty lines
            .skip(14)                                          // Skip gutenberg header
            .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
            .flatMap(line -> fromArray(line.split(" "))) // Next is in alternative
            // This scales but does not improve throughput !!!
            // .flatMap(line -> fromArray(line.split(" ")), Integer.MAX_VALUE, Integer.MAX_VALUE)
            .doOnNext(w -> {
                synchronized (mon) {
                    if (cont.value.length() < w.length()) {
                        cont.value = w;
                    }
                }
            });
    }
}
