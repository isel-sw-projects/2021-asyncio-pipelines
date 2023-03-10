package lookwords.Benchmark;

import lookwords.group.*;
import org.openjdk.jmh.annotations.Benchmark;
import org.openjdk.jmh.annotations.BenchmarkMode;
import org.openjdk.jmh.annotations.Mode;
import org.openjdk.jmh.annotations.OutputTimeUnit;
import org.openjdk.jmh.annotations.Param;
import org.openjdk.jmh.annotations.Scope;
import org.openjdk.jmh.annotations.State;

import java.util.Map;
import java.util.concurrent.TimeUnit;
import java.util.logging.Level;
import java.util.logging.Logger;

import static java.util.Collections.max;
import static java.util.Comparator.comparingInt;

@BenchmarkMode(Mode.AverageTime)
@OutputTimeUnit(TimeUnit.MILLISECONDS)
@State(Scope.Benchmark)
public class LookwordsGroupBench {

    @Param({"5"})
    private int min = 5;
    @Param({"10"})
    private int max = 10;
    @Param({"gutenberg"})
    private String folder = "guty"; // guty for unit tests purposes

    private static final Logger LOGGER = Logger.getLogger(LookwordsGroupBench.class.getPackageName());

    public LookwordsGroupBench() {
        System.setProperty(
            "java.util.logging.SimpleFormatter.format",
            "%4$s %2$s %5$s%6$s%n");
    }

    public LookwordsGroupBench(int min, int max, String folder) {
        this.min = min;
        this.max = max;
        this.folder = folder;
    }

    private Map.Entry<String, Integer> mostCommonWord(Map<String, Integer> words) {
        Map.Entry<String, Integer> common = max(words.entrySet(), comparingInt(Map.Entry::getValue));
        LOGGER.log(Level.INFO, () -> String.format(
            "The most common word with %d to %d chars is: %s. It occurs: %d times.%n",
            min,
            max,
            common.getKey(),
            common.getValue().intValue()
        ));
        return common;
    }


    @Benchmark
    public Map.Entry<String, Integer> blockingReaderInStreamsSequentially() {
        Map<String, Integer> words = new GroupWordsBlockingReaderInStreams(false).words(folder, min, max);
        return mostCommonWord(words);
    }

    @Benchmark
    public Map.Entry<String, Integer> bodyPublisherInFlux() {
        Map<String, Integer> words = new GroupWordsBodyPublisherInFlux().words(folder, min, max);
        return mostCommonWord(words);
    }

    @Benchmark
    public Map.Entry<String, Integer> bodyPublisherInObservable() {
        Map<String, Integer> words = new GroupWordsBodyPublisherInObservable().words(folder, min, max);
        return mostCommonWord(words);
    }

    @Benchmark
    public Map.Entry<String, Integer> blockingReaderInMultiThread() {
        Map<String, Integer> words = new GroupWordsBlockingReaderInMultiThread().words(folder, min, max);
        return mostCommonWord(words);
    }

    @Benchmark
    public Map.Entry<String, Integer> blockingReaderInStreamsParallel() {
        Map<String, Integer> words = new GroupWordsBlockingReaderInStreams(true).words(folder, min, max);
        return mostCommonWord(words);
    }

    @Benchmark
    public Map.Entry<String, Integer> rxIo() {
        Map<String, Integer> words = new GroupWordsRxIo().words(folder, min, max);
        return mostCommonWord(words);
    }

    @Benchmark
    public Map.Entry<String, Integer> rxIoInObservable() {
        Map<String, Integer> words = new GroupWordsRxIoInObservable().words(folder, min, max);
        return mostCommonWord(words);
    }

    @Benchmark
    public Map.Entry<String, Integer> rxIoInFlux() {
        Map<String, Integer> words = new GroupWordsRxIoInFlux().words(folder, min, max);
        return mostCommonWord(words);
    }

    @Benchmark
    public Map.Entry<String, Integer> rxIoInAsyncQuery() {
        Map<String, Integer> words = new GroupWordsRxIoInAsyncQuery().words(folder, min, max);
        return mostCommonWord(words);
    }

    @Benchmark
    public Map.Entry<String, Integer> groupWordsBaseLine() {
        Map<String, Integer> words = new GroupWordsBaseline().words(folder, min, max);
        return mostCommonWord(words);
    }
}
