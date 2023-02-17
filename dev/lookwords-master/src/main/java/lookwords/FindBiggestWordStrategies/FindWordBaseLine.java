package lookwords.FindBiggestWordStrategies;

import lookwords.FileUtils;
import org.javaync.io.AsyncFiles;
import org.jayield.AsyncQuery;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Optional;
import java.util.concurrent.*;
import java.util.stream.Stream;

import static java.util.Arrays.stream;
import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.bufferedReaderFrom;
import static lookwords.FileUtils.pathFrom;
import static reactor.core.publisher.Flux.fromArray;

public class FindWordBaseLine {


    public Optional<String> findBiggestWord(String folder) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {

            return paths
                    .filter(Files::isRegularFile)
                    .collect(toList())
                    .stream()
                    .map(file -> {
                        try {
                            return findWordInFile(file);
                        } catch (ExecutionException e) {
                            throw new RuntimeException(e);
                        } catch (InterruptedException e) {
                            throw new RuntimeException(e);
                        }
                    }).reduce(  (biggest, curr) -> curr.length() > biggest.length() ? curr : biggest);

        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    public String findWordInFile(Path file) throws ExecutionException, InterruptedException {
        CompletableFuture<String> allFileCharacters = AsyncFiles.readAll(file);
        String biggestWord = "";

        allFileCharacters.join();
        String[] lines = allFileCharacters.get().split("\n");

        for (int i = 14; i < lines.length; i++) {
            String curr = lines[i];
            if (curr.contains("*** END OF ")) {
                break;
            }

            String[] wordsInLine = curr.replaceAll("[^a-zA-Z ]", "").split(" ");

            for (int y = 0; y < wordsInLine.length; i++) {
                if (curr.length() >= biggestWord.length()) {
                    biggestWord = curr;
                }
            }
        }

        return biggestWord;
    }
}