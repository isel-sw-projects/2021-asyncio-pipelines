package lookwords.Benchmark;

import lookwords.FindBiggestWithParallel.FindBiggestWordParallelBlockingReaderInStreams;
import lookwords.FindBiggestWithParallel.FindBiggestWordParallelRxIoInFlux;
import lookwords.FindBiggestWithParallel.FindBiggestWordParallelRxIoInObservable;
import lookwords.FindBiggestWordStrategies.FindBiggestWordBlockingReaderInMultiThread;
import org.openjdk.jmh.annotations.*;

import java.util.concurrent.TimeUnit;
import java.util.logging.Level;
import java.util.logging.Logger;

@BenchmarkMode(Mode.AverageTime)
@OutputTimeUnit(TimeUnit.MILLISECONDS)
@State(Scope.Benchmark)
public class FindBiggestWordParallelBench {

    @Param({"gutenberg"})
    private String folder = "guty"; // guty for unit tests purposes

    private static final Logger LOGGER = Logger.getLogger(FindBiggestWordParallelBench.class.getPackageName());

    public FindBiggestWordParallelBench() {
        System.setProperty(
            "java.util.logging.SimpleFormatter.format",
            "%4$s %2$s %5$s%6$s%n");
    }

    public FindBiggestWordParallelBench(String folder) {
        this.folder = folder;
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
        String word = new FindBiggestWordParallelRxIoInObservable().findBiggestWord(folder);
        return biggestWord(word);
    }


    @Benchmark
    public String FindBiggestWordParallelRxIoInStream() {
        String word = new FindBiggestWordParallelBlockingReaderInStreams().findBiggestWord(folder);
        return biggestWord(word);
    }

    @Benchmark
    public String FindBiggestWordParallelRxIoInFlux() {
        String word = new FindBiggestWordParallelRxIoInFlux().findBiggestWord(folder);
        return biggestWord(word);
    }
    @Benchmark
    public String FindBiggestWordReaderInMultithread() {
        String word = new FindBiggestWordBlockingReaderInMultiThread().findBiggestWord(folder);
        return biggestWord(word);
    }


}
