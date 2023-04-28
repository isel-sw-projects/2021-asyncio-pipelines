package lookwords.FindBiggestWithParallel;

import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;
import static reactor.core.publisher.Flux.fromArray;

public class FindBiggestWordConcurrentRxIoInFlux implements FindBiggestWordConcurrent {

    public String findBiggestWord(String folder)  {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {

            return paths
                    .filter(Files::isRegularFile)
                    .collect(toList())
                    .stream()
                    .map( file -> findWordInFile(file))
                    .parallel()
                    .map( word -> word.block())
                    .reduce(  (biggest, curr) -> curr.length() > biggest.length() ? curr : biggest)
                    .get();

        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    public Mono<String> findWordInFile(Path file) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Flux
            .from(lines)
            .filter(line -> !line.isEmpty())                   // Skip empty lines
            .skip(14)                                          // Skip gutenberg header
            .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
            .flatMap(line -> fromArray(line.split(" ")))
            .reduce(  (biggest, curr) -> curr.length() > biggest.length() ? curr : biggest);
    }
}
