package lookwords.FindBiggestWordStrategies;

import kotlin.coroutines.Continuation;
import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Optional;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.stream.Stream;

import static java.util.Arrays.stream;
import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.*;

public class FindBiggestWordBlockingReaderInMultiThread implements FindBiggestWord {

     public final String findBiggestWord(String folder) {
         final int THREAD_POOL_SIZE = Runtime.getRuntime().availableProcessors();
         final ExecutorService EXEC = Executors.newFixedThreadPool(THREAD_POOL_SIZE);
         try (Stream<Path> paths = Files.walk(pathFrom(folder))) {

              return paths
                 .filter(Files::isRegularFile)
                 .map(file -> EXEC.submit(() -> biggestWord(file)))
                 .collect(toList())
                 .stream()
                 .map( future -> waitForFuture(future).get())
                 .reduce(  (biggest, curr) -> curr.length() > biggest.length() ? curr : biggest)
                 .get();

         } catch (IOException e) {
             throw new UncheckedIOException(e);
         } finally {
             EXEC.shutdown();
         }
     }

    public String biggestWord(Path file) {
        BufferedReader reader = bufferedReaderFrom(file);
        return reader
                .lines()
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> stream(line.replaceAll("[^a-zA-Z ]", "").split(" ")))
                .reduce( (biggest, curr) -> curr.length() > biggest.length() ? curr : biggest)
                .get();
    }


    private Optional<String> waitForFuture(Future<String> future) {
        try {
            return Optional.of(future.get());
        } catch (InterruptedException e) {
            e.printStackTrace();
        } catch (ExecutionException e) {
            e.printStackTrace();
        }
        return Optional.empty();

    }

}
