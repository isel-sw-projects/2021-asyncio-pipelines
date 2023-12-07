package lookwords.group;

import lookwords.FileUtils;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;
import java.util.Map;
import java.util.concurrent.*;
import java.util.stream.Stream;

import static java.util.Arrays.stream;
import static java.util.stream.Collectors.toList;

public class GroupWordsBlockingReaderInMultiThread implements GroupWords{
    /**
     * We use a lazy approach trying to avoid intermediate data structures.
     *
     */
    public final Map<String, Integer> words(Path folder, int minLength, int maxLength) {
        final int THREAD_POOL_SIZE = Runtime.getRuntime().availableProcessors();
        final ExecutorService EXEC = Executors.newFixedThreadPool(THREAD_POOL_SIZE);
        ConcurrentHashMap<String, Integer> words = new ConcurrentHashMap<>();
        try (Stream<Path> paths = Files.walk(folder)) {
            List<Callable<Void>> tasks = paths
                    .filter(Files::isRegularFile)
                    .map(file -> (Callable<Void>) () -> {
                        lines(file, minLength, maxLength)
                                .forEach(word -> words.merge(word, 1, Integer::sum));
                        return null;
                    })
                    .collect(toList());
            EXEC.invokeAll(tasks);
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        } catch (InterruptedException e) {
            // Handle exception
        } finally {
            EXEC.shutdown();
        }
        return words;
    }

    public static Stream<String> lines(Path file, int minLength, int maxLength) {
        BufferedReader reader = bufferedReaderFrom(file);
        return reader
                .lines()
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> stream(line.split(" ")))
                .filter(word -> word.length() > minLength && word.length() < maxLength)
                .onClose(() -> FileUtils.close(reader));
    }


    public static Path pathFrom(String file) {
        return Paths.get(file);
    }


    public static BufferedReader bufferedReaderFrom(Path path) {
        try {
            return Files.newBufferedReader(path);
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }



}
