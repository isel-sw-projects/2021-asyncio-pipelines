package lookwords.group;

import io.reactivex.rxjava3.core.Observable;
import io.reactivex.rxjava3.core.Single;
import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Stream;

import static io.reactivex.rxjava3.core.Observable.fromArray;
import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public class GroupWordsRXJava implements GroupWords{

    public final Map<String, Integer> words(String folder, int minLength, int maxLength) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            List<Single<HashMap<String, Integer>>> tasks = paths
                    .filter(Files::isRegularFile)
                    .map(path -> lines(path, minLength, maxLength))
                    .collect(toList());//each file in parallel

            Map<String, Integer> words = Single
                    .merge(tasks)
                    .reduce(new HashMap<String, Integer>(), (allWords, fileWords) -> {
                        fileWords.forEach((word, count) -> allWords.merge(word, count, Integer::sum));
                        return allWords;
                    })//merge results
                    .blockingGet(); // Get the result map
            return words;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

      Single<HashMap<String, Integer>> lines(Path file, int minLength, int maxLength) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Observable
                .fromPublisher(lines)
                .filter(line -> !line.isEmpty()) // Skip empty lines
                .skip(14) // Skip Gutenberg header
                .takeWhile(line -> !line.contains("*** END OF ")) // Skip Gutenberg footnote
                .flatMap(line -> Observable.fromArray(line.split(" "))) // Next is in alternative
                .filter(word -> word.length() > minLength && word.length() < maxLength)
                .reduce(new HashMap<String, Integer>(), (map, word) -> {
                    map.merge(word, 1, Integer::sum);
                    return map;
                });
    }


}
