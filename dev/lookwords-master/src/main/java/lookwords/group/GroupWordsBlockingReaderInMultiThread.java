package lookwords.group;

import lookwords.FileUtils;

import java.io.BufferedReader;
import java.io.File;
import java.io.IOException;
import java.io.UncheckedIOException;
import java.net.URISyntaxException;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;
import java.util.Map;
import java.util.concurrent.*;
import java.util.logging.Level;
import java.util.stream.Collectors;
import java.util.stream.Stream;

import static java.lang.ClassLoader.getSystemResource;
import static java.util.Arrays.stream;
import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.lines;
import static lookwords.FileUtils.pathFrom;
import static lookwords.group.GroupWordsBlockingReaderInStreams.LOGGER;

public class GroupWordsBlockingReaderInMultiThread implements GroupWords{
    /**
     * We use a lazy approach trying to avoid intermediate data structures.
     *
     */
    public final Map<String, Integer> words(String folder, int minLength, int maxLength) {
        final int THREAD_POOL_SIZE = Runtime.getRuntime().availableProcessors();
        final ExecutorService EXEC = Executors.newFixedThreadPool(THREAD_POOL_SIZE);
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            List<Future<Map<String, Integer>>> futures = paths
                    .filter(Files::isRegularFile)
                    .map(file -> EXEC.submit(() -> lines(file, minLength, maxLength)
                            .collect(Collectors.toMap(word -> word, word -> 1, Integer::sum))))
                    .collect(toList());

            Map<String, Integer> words = new ConcurrentHashMap<>();
            for (Future<Map<String, Integer>> future : futures) {
                try {
                    Map<String, Integer> result = future.get();
                    result.forEach((word, count) -> words.merge(word, count, Integer::sum));
                } catch (InterruptedException | ExecutionException e) {
                    // Handle the exception
                }
            }
            return words;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        } finally {
            EXEC.shutdown();
        }
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
        try {
            URL url = getSystemResource(file);
            return Paths.get(url.toURI());
        } catch (URISyntaxException e) {

        }
        return null;
    }


    public static BufferedReader bufferedReaderFrom(Path path) {
        try {
            return Files.newBufferedReader(path);
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }



}
