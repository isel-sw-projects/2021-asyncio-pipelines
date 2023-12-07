package lookwords.FindBiggestWordStrategies

import java.nio.file.Path

interface IFindBiggestWordWithFlow {
    suspend fun findBiggestWord(folder:Path) : String
}