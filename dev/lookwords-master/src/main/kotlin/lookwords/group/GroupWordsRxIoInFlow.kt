package lookwords.group

import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.async
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.runBlocking
import lookwords.FileUtils
import org.javaync.io.AsyncFiles
import java.nio.file.Files
import java.util.concurrent.ConcurrentHashMap
import kotlin.streams.asSequence

class GroupWordsRxIoInFlow : GroupWords{
   override fun words(folder: String, minLength: Int, maxLength: Int): Map<String, Int> {
        val words = ConcurrentHashMap<String, Int>()
        runBlocking {
            Files.walk(FileUtils.pathFrom(folder)).use { paths ->
                paths
                    .asSequence()
                        .filter { Files.isRegularFile(it) }
                        .map(AsyncFiles::flow)
                        .map { flow -> GlobalScope.async {
                              flow
                                      .filter { !it.isEmpty() }                   // Skip empty lines
                                      .drop(14)                                   // Skip gutenberg header
                                      .takeWhile { !it.contains("*** END OF ") }  // Skip gutenberg footnote
                                      .flatMapMerge { it.splitToSequence(" ").asFlow() }
                                      .filter{ it.length in (minLength + 1) until maxLength }
                                      .collect{ words.merge(it, 1, Integer::sum) }
                        } }
                        .toList()
                        .forEach{it.join()}
            }
        }
        return words
    }
}
