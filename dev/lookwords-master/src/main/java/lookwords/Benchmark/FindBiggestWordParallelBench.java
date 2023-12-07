package lookwords.Benchmark;

import lookwords.FindBiggestWithParallel.FindBiggestWordConcurrentBlockingReaderInStreams;
import lookwords.FindBiggestWithParallel.FindBiggestWordConcurrentRxIoInFlux;
import lookwords.FindBiggestWithParallel.FindBiggestWordConcurrentRxIoInObservable;
import lookwords.FindBiggestWordStrategies.FindBiggestWordBlockingReaderInMultiThread;
import org.openjdk.jmh.annotations.*;

import java.nio.file.Path;
import java.util.concurrent.TimeUnit;
import java.util.logging.Level;
import java.util.logging.Logger;

@BenchmarkMode(Mode.AverageTime)
@OutputTimeUnit(TimeUnit.MILLISECONDS)
@State(Scope.Benchmark)
public class FindBiggestWordParallelBench {

    private Path folder = Path.of("guty"); // guty for unit tests purposes

    private static final Logger LOGGER = Logger.getLogger(FindBiggestWordParallelBench.class.getPackageName());

    public FindBiggestWordParallelBench() {
        System.setProperty(
            "java.util.logging.SimpleFormatter.format",
            "%4$s %2$s %5$s%6$s%n");
    }

    public FindBiggestWordParallelBench(String folder) {
        this.folder = Path.of(folder);
    }

    private String biggestWord(String word) {

        LOGGER.log(Level.INFO, () -> String.format(
            "The biggest word found in the folder ' %s ' is: %s ",
            folder,
            word
        ));
        return word;
    }


    @Benchmark
    public String FindBiggestWordParallelRxIoInObservable() {
        String word = new FindBiggestWordConcurrentRxIoInObservable().findBiggestWord(folder);
        return biggestWord(word);
    }


    @Benchmark
    public String FindBiggestWordParallelRxIoInStream() {
        String word = new FindBiggestWordConcurrentBlockingReaderInStreams().findBiggestWord(folder);
        return biggestWord(word);
    }

    @Benchmark
    public String FindBiggestWordParallelRxIoInFlux() {
        String word = new FindBiggestWordConcurrentRxIoInFlux().findBiggestWord(folder);
        return biggestWord(word);
    }
    @Benchmark
    public String FindBiggestWordReaderInMultithread() {
        String word = new FindBiggestWordBlockingReaderInMultiThread().findBiggestWord(folder);
        return biggestWord(word);
    }


}
