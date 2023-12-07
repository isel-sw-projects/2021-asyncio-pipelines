package lookwords.group;

import io.reactivex.rxjava3.annotations.NonNull;
import io.reactivex.rxjava3.core.Observable;
import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Collectors;
import java.util.stream.Stream;


public class GroupWordsRXJava implements GroupWords{

    public final ConcurrentHashMap<String, Integer> words(Path folder, int minLength, int maxLength) {
        ConcurrentHashMap<String, Integer> map = new ConcurrentHashMap<>();
        try (Stream<Path> paths = Files.walk(folder)) {
            List<Observable<String>> tasks = paths
                    .filter(Files::isRegularFile)
                    .map(path -> lines(path, minLength, maxLength))
                    .collect(Collectors.toList());

            Observable.merge(tasks)
                    .blockingSubscribe(word -> map.merge(word, 1, Integer::sum));

            return map;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    Observable<String> lines(Path file, int minLength, int maxLength) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Observable
                .fromPublisher(lines)
                .filter(line -> !line.isEmpty()) // Skip empty lines
                .skip(14) // Skip Gutenberg header
                .takeWhile(line -> !line.contains("*** END OF ")) // Skip Gutenberg footnote
                .flatMap(line -> Observable.fromArray(line.split(" "))) // Next is in alternative
                .filter(word -> word.length() > minLength && word.length() < maxLength);
    }


}
