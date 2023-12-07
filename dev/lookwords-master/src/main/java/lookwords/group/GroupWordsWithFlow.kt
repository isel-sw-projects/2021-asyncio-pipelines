package lookwords.GroupWordsStrategies

import kotlinx.coroutines.async
import kotlinx.coroutines.awaitAll
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.launch
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
    fun wordsBlocking(folder: Path, minLength: Int, maxLength: Int): Map<String, Int> {
        return runBlocking {
            words(folder, minLength, maxLength)
        }
    }



    suspend fun words(folder: Path, minLength: Int, maxLength: Int): Map<String, Int> {
        val words = ConcurrentHashMap<String, Int>()
        try {
            Files.walk(folder).use { paths ->
                val files: List<Path> = paths
                    .filter { path: Path? -> Files.isRegularFile(path) }
                    .toList()

                coroutineScope {
                    files.forEach { file ->
                        launch { countWords(file, minLength, maxLength, words) }
                    }
                }
            }
        } catch (e: IOException) {
            throw UncheckedIOException(e)
        }

        return words
    }

    suspend fun countWords(file: Path, minLength: Int, maxLength: Int, words : ConcurrentHashMap<String, Int> ) {
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
    }

}
