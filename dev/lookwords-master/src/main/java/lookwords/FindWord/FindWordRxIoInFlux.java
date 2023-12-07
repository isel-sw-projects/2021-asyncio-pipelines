package lookwords.FindWord;

import lookwords.FileUtils;
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
import static reactor.core.publisher.Flux.fromArray;

public class FindWordRxIoInFlux {

    public Boolean findBiggestWord(String folder, String word) {
        try (Stream<Path> paths = Files.walk(FileUtils.FOLDER.resolve(folder))) {

            return paths
                    .filter(Files::isRegularFile)
                    .collect(toList())
                    .stream()
                    .map( file -> findWordInFile(file, word))
                    .map( wrd -> wrd.block())
                    .anyMatch( curr -> curr.equals(word));

        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    public Mono<Boolean> findWordInFile(Path file, String word) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Flux
                .from(lines)
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> fromArray(line.replaceAll("[^a-zA-Z ]", "").split(" ")))
                .any( curr -> curr.equals(word));
    }
}
