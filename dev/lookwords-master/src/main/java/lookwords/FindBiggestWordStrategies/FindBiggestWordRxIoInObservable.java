package lookwords.FindBiggestWordStrategies;

import io.reactivex.rxjava3.core.Maybe;
import io.reactivex.rxjava3.core.Observable;
import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;


import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.stream.Stream;

import static io.reactivex.rxjava3.core.Observable.fromArray;
import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

/**
 * Using the org.javasync.rxio library and its AsyncFiles utility class
 * built on top of java AsynchronousFileChannel to read a file.
 * Then the resulting Publisher is processed through a RxJava pipeline.
 */
public class FindBiggestWordRxIoInObservable implements FindBiggestWord {

    public String findBiggestWord(String folder) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {

            return paths
                    .filter(Files::isRegularFile)
                    .collect(toList())
                    .stream()
                    .map( file -> findWordInFile(file))
                    .map( word -> word.blockingGet())
                    .reduce(  (biggest, curr) -> curr.length() > biggest.length() ? curr : biggest)
                    .get();

        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }


    protected Maybe<String> findWordInFile(Path file) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Observable
                .fromPublisher(lines)
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> fromArray(line.replaceAll("[^a-zA-Z ]", "").split(" ")))
                .reduce(  (biggest, curr) -> curr.length() > biggest.length() ? curr : biggest);
    }
}