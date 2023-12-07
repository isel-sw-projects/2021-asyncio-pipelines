package lookwords.group;

import lookwords.FileUtils;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.UncheckedIOException;
import java.net.URISyntaxException;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.stream.Stream;

import static java.lang.ClassLoader.getSystemResource;
import static java.util.Arrays.stream;
import static lookwords.FileUtils.LOGGER;

/**
 * Here we are using Blocking IO through java Reader.
 */
public class GroupWordsBlockingReaderInStreams implements GroupWords {
    private final boolean parallel;

    public static final Logger LOGGER = Logger.getLogger(FileUtils.class.getPackageName());

    public GroupWordsBlockingReaderInStreams() {
        this(true);
    }

    public GroupWordsBlockingReaderInStreams(boolean parallel) {
        this.parallel = parallel;
    }



    public final Map<String, Integer> words(Path folder, int minLength, int maxLength) {
        ConcurrentHashMap<String, Integer> map = new ConcurrentHashMap<>();
        try (Stream<Path> paths = Files.walk(folder)) {
            Stream<Stream<String>> words = paths
                    .filter(Files::isRegularFile)
                    .map(file -> {
                        Stream<String> inner = lines(file, minLength, maxLength);
                        return parallel ? inner.parallel() : inner;
                    });
            if (parallel) words = words.parallel();

            words.forEach(wordStream -> wordStream.forEach(word -> map.merge(word, 1, Integer::sum)));
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
        return map;
    }

    public static Stream<String> lines(Path file, int minLength, int maxLength) {
        BufferedReader reader = bufferedReaderFrom(file);
        return reader
                .lines()
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> Stream.of(line.split(" ")))
                .filter(word -> word.length() > minLength && word.length() < maxLength)
                .onClose(() -> close(reader));
    }


public static Path pathFrom(String file) {
        return Paths.get(file);
    }
    /**
     * Reinier Zwitserloot who, as far as I know, had the first mention of this
     * technique in 2009 on the java posse mailing list.
     * http://www.mail-archive.com/javaposse@googlegroups.com/msg05984.html
     */
    static <T extends Throwable> T sneakyThrow(Throwable t) throws T{
        throw (T) t;
    }

    public static BufferedReader bufferedReaderFrom(Path path) {
        try {
            return Files.newBufferedReader(path);
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    public static <T> T join(Future<T> curr) {
        try {
            return curr.get();
        } catch (InterruptedException | ExecutionException e) {
            LOGGER.log(Level.WARNING, "Interrupted!", e);
            // Restore interrupted state...
            Thread.currentThread().interrupt();
        }
        return null;
    }

    public static void close(BufferedReader reader) {
        try {
            reader.close();
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }
}
