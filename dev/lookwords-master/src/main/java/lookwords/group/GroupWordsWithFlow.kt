package lookwords.GroupWordsStrategies

import kotlinx.coroutines.async
import kotlinx.coroutines.awaitAll
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.runBlocking
import lookwords.FileUtils
import org.javaync.io.AsyncFiles
import java.io.IOException
import java.io.UncheckedIOException
import java.nio.file.Files
import java.nio.file.Path
import java.util.concurrent.ConcurrentHashMap
import java.util.stream.Collectors
import kotlinx.coroutines.coroutineScope as coroutineScope

class GroupWordsWithFlow  {

    //for tests
    fun wordsBlocking(folder: String, minLength: Int, maxLength: Int): Map<String, Int> {
        return runBlocking {
            words(folder, minLength, maxLength)
        }
    }

    suspend fun words(folder: String, minLength: Int, maxLength: Int): Map<String, Int> {
        val words = mutableMapOf<String, Int>()

        try {
            Files.walk(FileUtils.pathFrom(folder)).use { paths ->
                val files: List<Path> = paths
                    .filter { path: Path? -> Files.isRegularFile(path) }
                    .collect(Collectors.toList())
                coroutineScope {
                    val deferreds = files.map { file ->
                        async { countWords(file, minLength, maxLength) }
                    }
                    deferreds.awaitAll().forEach { map ->
                        map.forEach { (k, v) ->
                            words.merge(k, v, Integer::sum)
                        }
                    }
                }
            }
        } catch (e: IOException) {
            throw UncheckedIOException(e)
        }

        return words
    }

    suspend fun countWords(file: Path, minLength: Int, maxLength: Int): Map<String, Int> {
        val words = mutableMapOf<String, Int>()

        val flow = AsyncFiles.flow(file)
        flow
            .filter { line: String -> line.isNotEmpty() } // Skip empty lines
            .drop(14) // Skip Gutenberg header
            .takeWhile { line: String -> !line.contains("*** END OF ") } // Skip Gutenberg footnote
            .flatMapConcat { line: String -> line.split(" ").asFlow() }
            .filter { word -> word.length in (minLength + 1) until maxLength }
            .collect { word ->
                words.merge(word, 1, Integer::sum)
            }

        return words
    }

}
