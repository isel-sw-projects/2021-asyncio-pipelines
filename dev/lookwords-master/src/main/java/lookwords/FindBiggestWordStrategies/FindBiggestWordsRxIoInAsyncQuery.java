package lookwords.FindBiggestWordStrategies;

import org.javaync.io.AsyncFiles;
import org.jayield.AsyncQuery;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.concurrent.CompletableFuture;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public class FindBiggestWordsRxIoInAsyncQuery implements FindBiggestBiggestWord {


    @Override
    public final Containner<String> findBiggestWord(String folder) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            Containner<String> cont = new Containner<>("");
            paths
                .filter(Files::isRegularFile)
                .map(path -> AsyncFiles
                    .asyncQuery(path)
                    .filter(line -> !line.isEmpty())                   // Skip empty lines
                    .skip(14)                                          // Skip gutenberg header
                    .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                    .flatMapMerge(line -> AsyncQuery.of(line.split(" ")))
                    .subscribe((w, err) ->
                            {
                                synchronized (this) {
                                    if(cont.value.length() < w.length()) {
                                        cont.value = w;
                                    }
                                }
                            })
                )
                .collect(toList())
                .forEach(CompletableFuture::join);

            return cont;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }


}
