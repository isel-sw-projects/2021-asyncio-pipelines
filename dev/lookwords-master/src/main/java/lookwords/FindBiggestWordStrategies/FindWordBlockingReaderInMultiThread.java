package lookwords.FindBiggestWordStrategies;

import lookwords.FileUtils;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.lines;
import static lookwords.FileUtils.pathFrom;

public class FindWordBlockingReaderInMultiThread implements FindWords {
    /**
     * We use a lazy approach trying to avoid intermediate data structures.
     * !!! To do: try it eagerly and check differences
     */
    public final Map<String, Integer> words(String folder, int minLength, int maxLength) {
        final int THREAD_POOL_SIZE = Runtime.getRuntime().availableProcessors();
        final ExecutorService EXEC = Executors.newFixedThreadPool(THREAD_POOL_SIZE);
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            ConcurrentHashMap<String, Integer> words = new ConcurrentHashMap<>();
            paths
                .filter(Files::isRegularFile)
                .map(file -> EXEC.submit(() -> lines(file, minLength, maxLength)
                    .forEach(word -> words.merge(word, 1, Integer::sum))))
                .collect(toList())
                .stream()
                .forEach(FileUtils::join);
            return words;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        } finally {
            EXEC.shutdown();
        }
    }
}
