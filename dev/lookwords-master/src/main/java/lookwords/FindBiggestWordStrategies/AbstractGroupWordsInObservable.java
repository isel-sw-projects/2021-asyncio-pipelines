package lookwords.FindBiggestWordStrategies;

import io.reactivex.rxjava3.core.Observable;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public abstract class AbstractGroupWordsInObservable implements FindBiggestBiggestWord {



    public final Containner<String> words(String folder) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            Containner<String> cont = new Containner<>("");

            List<Observable<String>> tasks = paths
                .filter(Files::isRegularFile)
                .map(path -> lines(path, cont))
                .collect(toList());
            Observable
                .merge(tasks)
                .blockingSubscribe();
            return cont;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    protected abstract Observable<String> lines(Path file, Containner<String> opt);
}
