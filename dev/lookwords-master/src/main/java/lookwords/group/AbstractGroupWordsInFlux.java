package lookwords.group;


import reactor.core.publisher.Flux;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public abstract class AbstractGroupWordsInFlux implements GroupWords{

    public final Map<String, Integer> words(String folder, int minLength, int maxLength) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            ConcurrentHashMap<String, Integer> words = new ConcurrentHashMap<>();
            List<Flux<String>> tasks = paths
                .filter(Files::isRegularFile)
                .map(path -> lines(path, minLength, maxLength, words))
                .collect(toList());
            Flux
                .merge(tasks)
                .blockLast();
            return words;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    protected abstract Flux<String> lines(Path file, int minLength, int maxLength, Map<String, Integer> words);
}
