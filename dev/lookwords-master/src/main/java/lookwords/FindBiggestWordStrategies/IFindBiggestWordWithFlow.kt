package lookwords.FindBiggestWordStrategies

interface IFindBiggestWordWithFlow {
    suspend fun findBiggestWord(folder:String) : String
}