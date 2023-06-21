package lookwords.group;


import org.javaync.io.AsyncFiles;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.Stream;

import static lookwords.FileUtils.pathFrom;
import static reactor.core.publisher.Flux.fromArray;

public  class GroupWordsInRectorCoreFlux implements GroupWords {

        @Override
        public final Map<String, Integer> words(String folder, int minLength, int maxLength) {
            try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
                List<Mono<HashMap<String, Integer>>> tasks = paths
                        .filter(Files::isRegularFile)
                        .map(path -> lines(path, minLength, maxLength))
                        .collect(Collectors.toList());
                return Flux
                        .merge(tasks)
                        .reduce(new HashMap<String, Integer>(), (allWords, fileWords) -> {
                            fileWords.forEach((word, count) -> allWords.merge(word, count, Integer::sum));
                            return allWords;
                        }).block();  // use block here to obtain Map
            } catch (IOException e) {
                throw new UncheckedIOException(e);
            }
        }

        // Rest of the class remains the same...



    protected Mono<HashMap<String, Integer>> lines(Path file, int minLength, int maxLength) {
        Publisher<String> lines = AsyncFiles.lines(file);
        return Flux
                .from(lines)
                .filter(line -> !line.isEmpty()) // Skip empty lines
                .skip(14) // Skip Gutenberg header
                .takeWhile(line -> !line.contains("*** END OF ")) // Skip Gutenberg footnote
                .flatMap(line -> fromArray(line.split(" "))) // Next is in alternative
                .filter(word -> word.length() > minLength && word.length() < maxLength)
                .reduce(new HashMap<String, Integer>(), (map, word) -> {
                    map.merge(word, 1, Integer::sum);
                    return map;
                });
    }




}
