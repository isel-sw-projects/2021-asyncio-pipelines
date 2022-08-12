package lookwords.FindBiggestWordStrategies;


import reactor.core.publisher.Flux;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public abstract class AbstractGroupWordsInFlux implements FindBiggestWord {

    public final Containner<String> findBiggestWord(String folder) {
        Containner<String> cont = new Containner<>("");
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {

            List<Flux<String>> tasks = paths
                .filter(Files::isRegularFile)
                .map(path -> lines(path, cont))
                .collect(toList());
            Flux
                .merge(tasks)
                .blockLast();

            return cont;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    protected abstract Flux<String> lines(Path file, Containner<String> opt);
}
