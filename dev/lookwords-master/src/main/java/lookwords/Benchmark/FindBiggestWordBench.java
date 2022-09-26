package lookwords.Benchmark;

import lookwords.FindBiggestWordStrategies.*;
import org.openjdk.jmh.annotations.*;

import java.util.concurrent.TimeUnit;
import java.util.logging.Level;
import java.util.logging.Logger;

import static java.util.Collections.max;

@BenchmarkMode(Mode.AverageTime)
@OutputTimeUnit(TimeUnit.MILLISECONDS)
@State(Scope.Benchmark)
public class FindBiggestWordBench {

    @Param({"gutenberg"})
    private String folder = "guty"; // guty for unit tests purposes

    private static final Logger LOGGER = Logger.getLogger(FindBiggestWordBench.class.getPackageName());

    public FindBiggestWordBench() {
        System.setProperty(
            "java.util.logging.SimpleFormatter.format",
            "%4$s %2$s %5$s%6$s%n");
    }

    public FindBiggestWordBench(String folder) {
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
    public String FindBiggestWordRxIoInObservable() {
        String word = new FindBiggestWordRxIoInObservable().findBiggestWord(folder);
        return biggestWord(word);
    }


    @Benchmark
    public String FindBiggestWordRxIoInStream() {
        String word = new FindBiggestWordBlockingReaderInStreams().findBiggestWord(folder);
        return biggestWord(word);
    }

    @Benchmark
    public String FindBiggestWordRxIoInFlux() {
        String word = new FindBiggestWordRxIoInFlux().findBiggestWord(folder);
        return biggestWord(word);
    }
    @Benchmark
    public String FindBiggestWordReaderInMultithread() {
        String word = new FindBiggestWordBlockingReaderInMultiThread().findBiggestWord(folder);
        return biggestWord(word);
    }

    @Benchmark
    public String FindBiggestWordReaderInFlow() {
        
        String word = (String) new FindBiggestWordWithFlow().findBiggestWord(folder);
        return biggestWord(word);
    }

}
