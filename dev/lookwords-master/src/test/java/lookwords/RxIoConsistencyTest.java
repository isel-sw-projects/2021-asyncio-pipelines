package lookwords;

import io.reactivex.rxjava3.core.Observable;
import org.javaync.io.AsyncFiles;
import org.junit.Test;

import java.io.IOException;
import java.net.URISyntaxException;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Iterator;

import static java.lang.ClassLoader.getSystemResource;
import static java.nio.charset.StandardCharsets.UTF_8;
import static org.junit.Assert.assertEquals;

public class RxIoConsistencyTest {

    @Test public void testRxIoAsyncQueryForPg1940() throws IOException {
        long actual = Files
            .lines(pathFrom("guty/pg1940.txt"), UTF_8)
            .count();
        assertEquals(1105, actual);

        Iterator<String> expected = Files
            .lines(pathFrom("guty/pg1940.txt"), UTF_8)
            .iterator();

        int [] count = { 0 };
        AsyncFiles
            .asyncQuery(pathFrom("guty/pg1940.txt"))
            .subscribe((line, err) -> {
                assertEquals(expected.next(), line);
                count[0]++;
            })
            .thenAccept(nothing -> assertEquals(false, expected.hasNext()))
            .join();
        assertEquals(false, expected.hasNext());
        assertEquals(1105, count[0]);
    }

    @Test public void testRxIoInObservableForPg1940() throws IOException {
        long actual = Files
            .lines(pathFrom("guty/pg1940.txt"), UTF_8)
            .count();
        assertEquals(1105, actual);

        Iterator<String> expected = Files
            .lines(pathFrom("guty/pg1940.txt"), UTF_8)
            .iterator();

        int [] count = { 0 };
        Observable
            .fromPublisher(AsyncFiles.lines(pathFrom("guty/pg1940.txt")))
            .doOnNext(line -> {
                assertEquals(expected.next(), line);
                count[0]++;
            })
            .doOnComplete(() -> assertEquals(false, expected.hasNext()))
            .blockingSubscribe();
        assertEquals(false, expected.hasNext());
        assertEquals(1105, count[0]);
    }

    private static Path pathFrom(String file) {
        try {
            URL url = getSystemResource(file);
            return Paths.get(url.toURI());
        } catch (URISyntaxException e) {
            throw new RuntimeException(e);
        }
    }
}
