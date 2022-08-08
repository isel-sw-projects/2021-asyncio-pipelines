package lookwords.FindBiggestWordStrategies;

import io.reactivex.rxjava3.core.Observable;

import java.io.FileNotFoundException;
import java.io.UncheckedIOException;
import java.net.http.HttpRequest.BodyPublishers;
import java.nio.ByteBuffer;
import java.nio.file.Path;
import java.util.Map;
import java.util.concurrent.Flow;

import static io.reactivex.rxjava3.core.Observable.fromArray;
import static io.reactivex.rxjava3.core.Observable.fromPublisher;
import static org.reactivestreams.FlowAdapters.toPublisher;

/**
 * Trying java.net.http.HttpRequest.BodyPublishers and his ofFile(),
 * which creates a Flow.Publisher of ByteBuffer.
 * This Publisher uses internally Blocking IO and an auxiliary thread pool.
 *
 * !!!!! Yet it is not scaling although it is using a similar Observable pipeline to that one used in RxIo.
 */
public class FindWordBodyPublisherInObservable extends AbstractGroupWordsInObservable {

    @Override
    protected Observable<String> lines(Path file, int minLength, int maxLength, Map<String, Integer> words) {
        ByteBufferToLines lines = new ByteBufferToLines();
        return fromPublisher(toPublisher(ofFile(file)))
                    .flatMap(lines::observable) // !!! Not Scale even with: .map(buffer -> new String(buffer.array()))
                    .filter(line -> !line.isEmpty())                   // Skip empty lines
                    .skip(14)                                          // Skip gutenberg header
                    .takeWhile(line -> !line.contains("*** END OF "))  // Skip gutenberg footnote
                    .flatMap(line -> fromArray(line.split(" ")))
                    .filter(word -> word.length() > minLength && word.length() < maxLength)
                    .doOnNext(w -> words.merge(w, 1, Integer::sum));
    }

    private static Flow.Publisher<ByteBuffer> ofFile(Path file) {
        try {
            return BodyPublishers.ofFile(file);
        } catch (FileNotFoundException e) {
            throw new UncheckedIOException(e);
        }
    }


    private static class ByteBufferToLines {

        final StringBuilder prev = new StringBuilder();

        public final Observable<String> observable(ByteBuffer chunk) {
            prev.append(new String(chunk.array())); // join previous with current
            if(prev.indexOf("\n") < 0)              // return empty if there are no lines delimiters
                return Observable.empty();
            String[] lines = prev.toString().split("\n|(\r\n)");     // split in lines
            prev.setLength(0);                                       // clear the builder
            if(lines.length > 1) prev.append(lines[lines.length-1]); // append the last statement
            return lines.length > 1
                ? fromArray(lines).take(lines.length - 1L) // drop last statement
                : fromArray(lines);
        }
    }

}
