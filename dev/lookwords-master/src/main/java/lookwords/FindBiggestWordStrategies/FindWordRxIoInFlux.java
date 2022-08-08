package lookwords.FindBiggestWordStrategies;

import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;

import java.nio.file.Path;
import java.util.Map;

import static reactor.core.publisher.Flux.fromArray;

public class FindWordRxIoInFlux extends AbstractGroupWordsInFlux {
    @Override
    protected Flux<String> lines(Path file, int minLength, int maxLength, Map<String, Integer> words) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Flux
            .from(lines)
            .filter(line -> !line.isEmpty())                   // Skip empty lines
            .skip(14)                                          // Skip gutenberg header
            .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
            .flatMap(line -> fromArray(line.split(" "))) // Next is in alternative
            // This scales but does not improve throughput !!!
            // .flatMap(line -> fromArray(line.split(" ")), Integer.MAX_VALUE, Integer.MAX_VALUE)
            .filter(word -> word.length() > minLength && word.length() < maxLength)
            .doOnNext(w -> words.merge(w, 1, Integer::sum));
    }
}
