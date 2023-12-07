package lookwords.FindWord;

import lookwords.FileUtils;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.stream.Stream;

import static java.util.Arrays.stream;
import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.*;

public class FindWordBlockingReaderInMultiThread {

    public final Boolean findBiggestWord(String folder, String word) {
        final int THREAD_POOL_SIZE = Runtime.getRuntime().availableProcessors();
        final ExecutorService EXEC = Executors.newFixedThreadPool(THREAD_POOL_SIZE);
        try (Stream<Path> paths = Files.walk(FOLDER.resolve(folder))) {

            return paths
                    .filter(Files::isRegularFile)
                    .map(file -> EXEC.submit(() -> biggestWord(file,word)))
                    .collect(toList())
                    .stream()
                    .map( future -> waitForFuture(future))
                    .anyMatch(curr -> curr);

        } catch (IOException e) {
            throw new UncheckedIOException(e);
        } finally {
            EXEC.shutdown();
        }
    }

    public boolean biggestWord(Path file, String word) {
        BufferedReader reader = bufferedReaderFrom(file);
        return reader
                .lines()
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> stream(line.replaceAll("[^a-zA-Z ]", "").split(" ")))
                .anyMatch( curr -> curr.equals(word));
    }


    private boolean waitForFuture(Future<Boolean> future) {
        try {
            return future.get();
        } catch (InterruptedException e) {
            e.printStackTrace();
        } catch (ExecutionException e) {
            e.printStackTrace();
        }
        return false;
    }
}
