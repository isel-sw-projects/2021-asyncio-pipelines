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
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.stream.Stream;

import static java.util.Arrays.stream;
import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.bufferedReaderFrom;
import static lookwords.FileUtils.pathFrom;
import static reactor.core.publisher.Flux.fromArray;

public class FindWordBaseLine {


    public Boolean findBiggestWord(String folder, String word) {
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {

            return paths
                    .filter(Files::isRegularFile)
                    .collect(toList())
                    .stream()
                    .map( file -> findWordInFile(file, word))
                    .map( wrd -> wrd.block())
                    .anyMatch( curr -> curr.equals(word));

        } catch (IOException e) {
            throw new UncheckedIOException(e);
        }
    }

    public Mono<String> findWordInFile(Path file, String word) {
        AsyncQuery<String> lines = AsyncFiles.asyncQuery(file);

        return lines
                .skip(14)
                .onNext( line -> {

                    String biggestWord = "";
                    if (line.contains("*** END OF ")) {
                        return;
                    }

                    String[] wordsInLine = line.replaceAll("[^a-zA-Z ]", "").split(" ");


                    for(int i = 0; i< wordsInLine.length; i++){
                        if (word.length() >= biggestWord.length()) {
                            biggestWord = word;
                        }
                    }

                    return biggestWord;

                });

    }
}
