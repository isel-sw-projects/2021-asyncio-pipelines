package lookwords.group;

import org.javaync.io.AsyncFiles;
import org.jayield.AsyncQuery;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Map;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public class GroupWordsRxIoInAsyncQuery implements GroupWords {
    public final Map<String, Integer> words(String folder, int minLength, int maxLength) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            ConcurrentHashMap<String, Integer> words = new ConcurrentHashMap<>();
            paths
                .filter(Files::isRegularFile)
                .map(path -> AsyncFiles
                    .asyncQuery(path)
                    .filter(line -> !line.isEmpty())                   // Skip empty lines
                    .skip(14)                                          // Skip gutenberg header
                    .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                    .flatMapMerge(line -> AsyncQuery.of(line.split(" ")))
                    .filter(word -> word.length() > minLength && word.length() < maxLength)
                    .subscribe((w, err) -> words.merge(w, 1, Integer::sum))
                )
                .collect(toList())
                .forEach(CompletableFuture::join);
            return words;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }
}
