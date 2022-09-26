package lookwords.FindWord;

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
public class FindWordBlockingReaderInStreams {

    public final boolean findBiggestWord(String folder,String word) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            return paths
                .filter(Files::isRegularFile)
                .map(file -> findWordInFile(file, word))
                .anyMatch(curr -> curr.equals(word));

        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    public static boolean findWordInFile(Path file, String word) {
        BufferedReader reader = bufferedReaderFrom(file);
        return reader
                .lines()
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> stream(line.replaceAll("[^a-zA-Z ]", "").split(" ")))
                .anyMatch( curr -> curr.equals(word));
    }

}
