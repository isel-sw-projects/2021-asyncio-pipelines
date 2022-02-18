package lookwords.group;

import org.javaync.io.AsyncFiles;
import org.reactivestreams.Subscriber;
import org.reactivestreams.Subscription;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Map;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.BiConsumer;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public class GroupWordsRxIo implements GroupWords {

    public final Map<String, Integer> words(String folder, int minLength, int maxLength) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            ConcurrentHashMap<String, Integer> words = new ConcurrentHashMap<>();
            paths
                .filter(Files::isRegularFile)
                .map(path -> readInto(path, minLength, maxLength, words))
                .collect(toList())
                .stream()
                .forEach(CompletableFuture::join);
            return words;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    private final CompletableFuture<Void> readInto(Path file, int minLength, int maxLength, Map<String, Integer> words) {
        CompletableFuture<Void> cf = new CompletableFuture<>();
        class GutenbergLinesParser {
            int count = 0;
            boolean finish = false;
            public void parse(String line, Throwable err) {
                if(finish) return;
                if(err != null) {
                    finish = true;
                    cf.completeExceptionally(err);
                    return;
                }
                if (line == null || line.contains("*** END OF ")) {
                    finish = true;
                    cf.complete(null);
                    return;
                }
                if(!line.isEmpty()) {
                    if(count > 14) {
                        for(String w : line.split(" "))
                            if(w.length() > minLength && w.length() < maxLength)
                                words.merge(w, 1, Integer::sum);
                    } else {
                        count++;
                    }
                }
            }
        }
        readInto(file, new GutenbergLinesParser()::parse);
        return cf;
    }

    /**
     * !!!! MISSING cancel Subscription !!!!
     */
    private static void readInto(Path file, BiConsumer<String, Throwable> parser) {
        AsyncFiles
            .lines(file)
            .subscribe(new Subscriber<String>() {
                public void onSubscribe(Subscription s) { s.request(Integer.MAX_VALUE);}
                public void onError(Throwable t) { parser.accept(null, t); }
                public void onComplete() { parser.accept(null, null); }
                public void onNext(String line) { parser.accept(line, null);}
            });
    }

}
