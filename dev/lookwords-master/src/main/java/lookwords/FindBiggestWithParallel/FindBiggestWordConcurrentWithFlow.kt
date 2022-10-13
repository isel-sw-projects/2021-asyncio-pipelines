package lookwords.FindBiggestWithParallel

import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import lookwords.FileUtils
import lookwords.FindBiggestWordStrategies.IFindBiggestWordWithFlow
import org.javaync.io.AsyncFiles
import java.io.IOException
import java.io.UncheckedIOException
import java.nio.file.Files
import java.nio.file.Path
import java.util.stream.Collectors

class FindBiggestWordConcurrentWithFlow : IFindBiggestWordWithFlow {

      override suspend fun findBiggestWord(folder:String) : String = coroutineScope{ // flow builder

        try {
            Files.walk(FileUtils.pathFrom(folder)).use { paths ->
                var pths: List<Path>  = paths
                    .filter { path: Path? -> Files.isRegularFile(path) }
                    .collect(Collectors.toList());

                val word: Deferred<String> = pths
                    .map { curr -> async(Dispatchers.IO) { findWord(curr) } }
                    .reduce { biggest, curr -> if (curr.await().length > biggest.await().length) curr else biggest }
                word.await()

            }
        }catch (e: IOException) {
            throw UncheckedIOException(e)
        }
    }

    suspend fun findWord(file: Path) : String {
        val flow = AsyncFiles.flow(file)
        return flow
            .filter { line: String -> !line.isEmpty() } // Skip empty lines
            .drop(14) // Skip gutenberg header
            .takeWhile { line: String -> !line.contains("*** END OF ") } // Skip gutenberg footnote
            .flatMapConcat { line: String -> line.split(" ").asFlow() }
            .filter { word -> word != null }
            .reduce { biggest: String, curr: String ->
                if (curr.length > biggest.length)
                    curr
                else
                    biggest
            }
    }
}


