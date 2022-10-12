package lookwords.FindBiggestWordStrategies

import kotlinx.coroutines.flow.*
import lookwords.FileUtils
import org.javaync.io.AsyncFiles
import java.io.IOException
import java.io.UncheckedIOException
import java.nio.file.Files
import java.nio.file.Path
import java.util.stream.Collectors
import kotlin.reflect.KFunction1

class FindBiggestWordWithFlow : IFindBiggestWordWithFlow  {

      override suspend fun findBiggestWord(folder:String) : String { // flow builder

        try {
            Files.walk(FileUtils.pathFrom(folder)).use { paths ->
                var pths: List<Path>  = paths
                    .filter { path: Path? -> Files.isRegularFile(path) }
                    .collect(Collectors.toList());

              return  pths
                  .map{AsyncFiles::flow }
                    .map{ curr -> findWord(curr)}
                    .reduce { biggest: String, curr: String -> if (curr.length > biggest.length) curr else biggest }

            }
        }catch (e: IOException) {
            throw UncheckedIOException(e)
        }
    }

    suspend fun findWord(flow: KFunction1<Path, Flow<String>>) : String {
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


