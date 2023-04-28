package lookwords.FindBiggestWordStrategies;

import lookwords.FindBiggestWithParallel.FindBiggestWordConcurrent;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.stream.Stream;

import static java.util.Arrays.stream;
import static lookwords.FileUtils.*;

/**
 * Here we are using Blocking IO through java Reader.
 */
public class FindBiggestWordBlockingReaderInStreams implements FindBiggestWordConcurrent {

    public final String findBiggestWord(String folder)  {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            String word = paths
                .filter(Files::isRegularFile)
                .flatMap(file  -> findBiggestWordInFile(file))
                .reduce(  (biggest, curr) -> curr.length() > biggest.length() ? curr : biggest)
                .get();


           return word;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    public static Stream<String> findBiggestWordInFile(Path file) {
        BufferedReader reader = bufferedReaderFrom(file);
        return reader
                .lines()
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> stream(line.split(" ")))
                .reduce((biggest, curr) -> curr.length() > biggest.length() ? curr : biggest)
                .stream()
                .onClose(() -> close(reader));
    }
}
