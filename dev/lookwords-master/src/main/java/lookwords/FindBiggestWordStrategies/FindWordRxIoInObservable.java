package lookwords.FindBiggestWordStrategies;

import io.reactivex.rxjava3.core.Observable;
import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;

import java.nio.file.Path;
import java.util.Map;
import java.util.Optional;

import static io.reactivex.rxjava3.core.Observable.fromArray;

/**
 * Using the org.javasync.rxio library and its AsyncFiles utility class
 * built on top of java AsynchronousFileChannel to read a file.
 * Then the resulting Publisher is processed through a RxJava pipeline.
 */
public class FindWordRxIoInObservable {
    Object mon = new Object();
    protected  Observable<String> lines(Path file , Containner<String> cont) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Observable
            .fromPublisher(lines)
            .filter(line -> !line.isEmpty())                   // Skip empty lines
            .skip(14)                                          // Skip gutenberg header
            .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
            .flatMap(line -> fromArray(line.split(" ")))
            .doOnNext(w -> {
                synchronized (mon) {
                    if (cont.value.length() < w.length()) {
                        cont.value = w;
                    }
                }
            });
    }
}
