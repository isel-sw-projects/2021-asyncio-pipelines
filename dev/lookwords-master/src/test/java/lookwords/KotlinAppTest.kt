package lookwords

import kotlinx.coroutines.runBlocking
import lookwords.FindBiggestWithParallel.FindBiggestWordParallelWithFlow
import org.junit.Test
import java.time.Instant
import java.util.logging.Level
import java.util.logging.Logger

class KotlinAppTest {
    private val LOGGER = Logger.getLogger(AppTest::class.java.packageName)
    val ITERATIONS = 5
    private val FOLDER = "gutenberg"
    @Test
    fun testMain()
    {
        runBlocking {
            testGroupingFindBiggestFlow(FindBiggestWordParallelWithFlow())
        }
    }

   suspend fun performFindBiggestKotlin(task: FindBiggestWordParallelWithFlow): String? {
        LOGGER.log(Level.INFO, "############ {0}", task.javaClass)
        var res: String? = null
        var minTime = Long.MAX_VALUE
        for (i in 0 until AppTest.ITERATIONS) {
            val startTime = Instant.now()
            res = task.findBiggestWord(FOLDER)
            val dur = AppTest.between(startTime)
            LOGGER.log(Level.INFO, "time: {0} ms", dur)
            if (dur < minTime) minTime = dur
        }
        LOGGER.log(Level.INFO, "=====> BEST: {0} ms", minTime)
        return res
    }

    suspend fun testGroupingFindBiggestFlow(task: FindBiggestWordParallelWithFlow?) {
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
}