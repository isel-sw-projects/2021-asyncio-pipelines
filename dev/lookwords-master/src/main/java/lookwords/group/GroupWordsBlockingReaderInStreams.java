package lookwords.group;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Stream;

import static lookwords.FileUtils.lines;
import static lookwords.FileUtils.pathFrom;

/**
 * Here we are using Blocking IO through java Reader.
 */
public class GroupWordsBlockingReaderInStreams implements GroupWords {
    private final boolean parallel;

    public GroupWordsBlockingReaderInStreams() {
        this(true);
    }

    public GroupWordsBlockingReaderInStreams(boolean parallel) {
        this.parallel = parallel;
    }

    public final Map<String, Integer> words(String folder, int minLength, int maxLength) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            Stream<Stream<String>> words = paths
                .filter(Files::isRegularFile)
                .map(file  -> {
                    var inner = lines(file, minLength, maxLength);
                    return parallel ? inner.parallel() : inner;
                });
            if(parallel) words = words.parallel();
            /**
             * An alternative that does not scale and it is harmful: words.flatMap(identity()).collect(groupingBy(identity(), counting()));
             * The following one performs better...
             */
            return words.collect(
                ConcurrentHashMap::new,
                (map, strm) -> strm.forEach(word -> map.merge(word, 1, Integer::sum)),
                (m1, m2) -> m2.forEach((k, v) -> m1.merge(k, v, Integer::sum)));
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

}
