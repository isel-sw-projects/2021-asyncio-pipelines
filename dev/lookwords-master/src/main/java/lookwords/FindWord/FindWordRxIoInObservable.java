package lookwords.FindWord;

import io.reactivex.rxjava3.core.Observable;
import io.reactivex.rxjava3.core.Single;
import lookwords.FileUtils;
import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.stream.Stream;

import static io.reactivex.rxjava3.core.Observable.fromArray;
import static java.util.stream.Collectors.toList;
/**
 * Using the org.javasync.rxio library and its AsyncFiles utility class
 * built on top of java AsynchronousFileChannel to read a file.
 * Then the resulting Publisher is processed through a RxJava pipeline.
 */
public class FindWordRxIoInObservable {

    public boolean findBiggestWord(String folder,String word) {
        try (Stream<Path> paths = Files.walk(FileUtils.FOLDER.resolve(folder))) {

            return paths
                    .filter(Files::isRegularFile)
                    .collect(toList())
                    .stream()
                    .map( file -> findWordInFile(file, word))
                    .anyMatch( curr -> curr.equals(word));

        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }


    protected Single<Boolean> findWordInFile(Path file, String word) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Observable
                .fromPublisher(lines)
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> fromArray(line.replaceAll("[^a-zA-Z ]", "").split(" ")))
                .any( curr -> curr.equals(word));
    }
}
