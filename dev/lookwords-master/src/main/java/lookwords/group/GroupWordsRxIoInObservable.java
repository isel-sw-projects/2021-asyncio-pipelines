package lookwords.group;

import io.reactivex.rxjava3.core.Observable;
import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;

import java.nio.file.Path;
import java.util.Map;

import static io.reactivex.rxjava3.core.Observable.fromArray;

/**
 * Using the org.javasync.rxio library and its AsyncFiles utility class
 * built on top of java AsynchronousFileChannel to read a file.
 * Then the resulting Publisher is processed through a RxJava pipeline.
 */
public class GroupWordsRxIoInObservable extends AbstractGroupWordsInObservable {

    protected  Observable<String> lines(Path file, int minLength, int maxLength, Map<String, Integer> words) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Observable
            .fromPublisher(lines)
            .filter(line -> !line.isEmpty())                   // Skip empty lines
            .skip(14)                                          // Skip gutenberg header
            .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
            .flatMap(line -> fromArray(line.split(" ")))
            .filter(word -> word.length() > minLength && word.length() < maxLength)
            .doOnNext(w -> words.merge(w, 1, Integer::sum));
    }
}
