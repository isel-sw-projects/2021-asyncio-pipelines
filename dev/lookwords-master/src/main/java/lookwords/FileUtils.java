package lookwords;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.UncheckedIOException;
import java.net.URISyntaxException;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Optional;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.stream.Stream;

import static java.lang.ClassLoader.getSystemResource;
import static java.util.Arrays.stream;

public class FileUtils {

    private FileUtils() {
    }

    public static final Logger LOGGER = Logger.getLogger(FileUtils.class.getPackageName());

    /**
     * Following a lazy approach and trying to avoid intermediate data structures.
     */

    public static Stream<String> lines(Path file, int minLength, int maxLength) {
        BufferedReader reader = bufferedReaderFrom(file);
        return reader
                .lines()
                .filter(line -> !line.isEmpty())                   // Skip empty lines
                .skip(14)                                          // Skip gutenberg header
                .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                .flatMap(line -> stream(line.split(" ")))
                .filter(word -> word.length() > minLength && word.length() < maxLength)
                .onClose(() -> close(reader));
    }


    public static Path pathFrom(String file) {
        try {
            URL url = getSystemResource(file);
            return Paths.get(url.toURI());
        } catch (URISyntaxException e) {
            throw sneakyThrow(e);
        }
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
