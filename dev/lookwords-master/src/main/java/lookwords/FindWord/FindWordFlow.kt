package lookwords.FindWord

import kotlinx.coroutines.flow.*
import lookwords.FileUtils
import org.javaync.io.AsyncFiles
import java.io.IOException
import java.io.UncheckedIOException
import java.nio.file.Files
import java.nio.file.Path
import java.util.stream.Collectors

class FindWordFlow {
    suspend fun findBiggestWord(folder:String, word:String) : Boolean { // flow builder

        try {
            Files.walk(FileUtils.FOLDER.resolve(folder)).use { paths ->
                var pths: List<Path>  = paths
                    .filter { path: Path? -> Files.isRegularFile(path) }
                    .collect(Collectors.toList());

                return  pths.map{ curr -> findWord(curr)}
                    .any { curr: String -> curr==word }

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
            .filter { item -> item != null }
            .reduce { biggest: String, curr: String ->
                if (curr.length > biggest.length)
                    curr
                else
                    biggest
            }
        }
    }