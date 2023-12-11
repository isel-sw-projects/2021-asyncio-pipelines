package AsyncFile

import AppTest
import kotlinx.coroutines.runBlocking
import lookwords.FileUtils
import lookwords.FindBiggestWordStrategies.FindBiggestWordWithFlow
import lookwords.FindBiggestWordStrategies.IFindBiggestWordWithFlow
import lookwords.GroupWordsStrategies.GroupWordsWithFlow
import java.time.Duration

import java.time.Instant
import java.util.logging.Level
import java.util.logging.Logger

class KotlinAppTest {
    private val LOGGER = Logger.getLogger(AppTest::class.java.packageName)

    fun testMain()
    {
        runBlocking {
           // performFindBiggestKotlin(FindBiggestWordConcurrentWithFlow())
            performFindBiggestKotlin(FindBiggestWordWithFlow())
         //  performFindBiggestKotlin(FindBiggestWordWithFlowIOBlocking())

            testGroupingKotlin(GroupWordsWithFlow())
        }
    }

   suspend fun performFindBiggestKotlin(task: IFindBiggestWordWithFlow): String? {
        LOGGER.log(Level.INFO, "############ {0}", task.javaClass)
        var res: String? = null
        var minTime = Long.MAX_VALUE

       var sumTime = 0L
       var count = 0

        for (i in 0 until 5) {
            val startTime = Instant.now()
            res = task.findBiggestWord(FileUtils.FOLDER)
            val dur = between(startTime)
            println("Biggest word is: $res")
            LOGGER.log(Level.INFO, "time: {0} ms", dur)
            if (dur < minTime) minTime = dur
            if (i > 0) { // Skip first iteration for averaging
                sumTime += dur
                count++
            }
        }

        val averageTime = if (count > 0) sumTime / count else 0 // Compute average
        LOGGER.log(Level.INFO, "=====> AVERAGE: {0} ms", averageTime)
        LOGGER.log(Level.INFO, "=====> BEST: {0} ms\n", minTime)
        return res
    }

    suspend fun performWordsKotlin(task: GroupWordsWithFlow): Map<String, Int>? {
        LOGGER.log(Level.INFO, "############ {0}", task.javaClass)
        var res: Map<String, Int>? = null
        var sumTime = 0L
        var count = 0

        for (i in 0 until 5) { // Change to 4 iterations
            val startTime = Instant.now()
            res = task.words(FileUtils.FOLDER, 5, 10)
            System.out.println(res.maxByOrNull { it.value }?.value)
            val dur = between(startTime)
            LOGGER.log(Level.INFO, "time: {0} ms", dur)

            if (i > 0) { // Skip first iteration for averaging
                sumTime += dur
                count++
            }
        }

        val averageTime = if (count > 0) sumTime / count else 0 // Compute average
        LOGGER.log(Level.INFO, "=====> AVERAGE: {0} ms\n", averageTime)
        return res
    }


    suspend fun testGroupingKotlin(task: GroupWordsWithFlow) {
        val words = performWordsKotlin(task)
        if (words == null) {
            LOGGER.log(Level.INFO, "NO results!")
            return
        }

        val common = words.maxByOrNull { it.value }
        common?.let {
            LOGGER.log(Level.INFO) {
                String.format(
                    "The most common word with %d to %d chars is: %s. It occurs: %d times.%n",
                    5,
                    10,
                    common.key,
                    common.value
                )
            }
        }
    }


    suspend fun testGroupingFindBiggestParallelFlow(task: IFindBiggestWordWithFlow?) {
        val word = performFindBiggestKotlin(task!!)
        if (word == null) {
            LOGGER.log(Level.INFO, "NO results!")
            return
        }
        LOGGER.log(Level.INFO) {
            String.format(
                "The biggest word found is: %s. ",
                word
            )
        }
    }

    fun between(startTime: Instant?): Long {
        return Duration.between(startTime, Instant.now()).toMillis()
    }

}
