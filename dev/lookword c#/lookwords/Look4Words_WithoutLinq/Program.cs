

using Look4Words_WithoutLinq;

namespace noLinq_lookwords
{
    class Program
    {

       public static void Main(string[] args)
       {
           string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");
           EnumerableIOFileReadWithoutLinqStrategy enumerableIOFileReadWithoutLinqStrategy = new EnumerableIOFileReadWithoutLinqStrategy();
      
            Console.WriteLine("Test initiated");
      
           int init = Environment.TickCount;
           enumerableIOFileReadWithoutLinqStrategy.countWordsFromFileSync(folderPath, 2, 12);
      
           int elapsed = Environment.TickCount - init;
      
           Console.WriteLine(@"Count all words RunSyncTest without linq took: {0} miliseconds", elapsed);
      
       }
    }
}
