package lookwords.Benchmark

import kotlinx.coroutines.flow.*
import lookwords.FindBiggestWordStrategies.FindBiggestWordBlockingReaderInMultiThread
import lookwords.FindBiggestWordStrategies.FindBiggestWordRxIoInFlux
import org.openjdk.jmh.annotations.*
import java.util.concurrent.TimeUnit
import java.util.logging.Level
import java.util.logging.Logger

class FlowBenchmark {


    @BenchmarkMode(Mode.AverageTime)
    @OutputTimeUnit(TimeUnit.MILLISECONDS)
    @State(Scope.Benchmark)
    class FlowBenchMark {

        @Param("gutenberg")
        private var folder = "guty" // guty for unit tests purposes


        private val LOGGER = Logger.getLogger(lookwords.Benchmark.FindBiggestWordBench::class.java.packageName)


        private fun biggestFlowWord(word: String): String {
            LOGGER.log(Level.INFO) {
                String.format(
                    "The biggest word found in the folder ' %s ' is: %s ",
                    folder,
                    word
                )
            }
            return word
        }

        fun FindBiggestWordBench() {
            System.setProperty(
                "java.util.logging.SimpleFormatter.format",
                "%4\$s %2\$s %5\$s%6\$s%n"
            )
        }

        fun FindBiggestWordBench(folder: String) {
            this.folder = folder
        }

        //  @Benchmark
        //   fun FindBiggestWordParallelRxIoInFlux(): String {
        //      val word: String = FindBiggestWordParallelRxIoInFlux().findWord(folder)
        //      return biggestFlowWord(word)
        //  }

        @Benchmark
        fun FindBiggestWordReaderInMultithread(): String? {
            val word = FindBiggestWordBlockingReaderInMultiThread().findBiggestWord(folder)
            return biggestFlowWord(word)
        }

        @Benchmark
        fun FindBiggestWordReaderInObservable(): String? {
            val word = FindBiggestWordParallelBench().FindBiggestWordParallelRxIoInObservable()
            return biggestFlowWord(word)
        }

    }
}