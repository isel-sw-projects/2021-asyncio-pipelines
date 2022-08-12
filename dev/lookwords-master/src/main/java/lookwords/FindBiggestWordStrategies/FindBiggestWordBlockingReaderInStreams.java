package lookwords.FindBiggestWordStrategies;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.concurrent.CompletableFuture;
import java.util.stream.Collectors;
import java.util.stream.Stream;

import static lookwords.FileUtils.lines;
import static lookwords.FileUtils.pathFrom;

/**
 * Here we are using Blocking IO through java Reader.
 */
public class FindBiggestWordBlockingReaderInStreams {
    private final boolean parallel;

    public FindBiggestWordBlockingReaderInStreams() {
        this(true);
    }

    public FindBiggestWordBlockingReaderInStreams(boolean parallel) {
        this.parallel = parallel;
    }


    public final Containner<String> words(String folder) {
        Containner<String> containner = new Containner<>("");
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            List<CompletableFuture<Void>> words = paths
                .filter(Files::isRegularFile)
                .map(file  -> lines(file, containner))
                    .collect(Collectors.toList());

           words.wait();

           return containner;
        } catch (IOException e) {
            throw new UncheckedIOException(e);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        return null;
    }

}
