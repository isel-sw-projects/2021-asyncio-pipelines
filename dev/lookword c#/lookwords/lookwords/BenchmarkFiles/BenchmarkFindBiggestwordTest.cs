using BenchmarkDotNet.Attributes;
using lookwords.BiggestWordFileReadStrategies.AsyncEnum;
using lookwords.BiggestWordFileReadStrategies.SyncEnum;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.BenchmarkFiles
{
    internal class BenchmarkFindBiggestwordTest
    {

        static private string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");
        

  //
  //      private void printResult(ConcurrentDictionary<string, int> dict)
  //      {
  //          foreach (var item in dict)
  //          {
  //              {
  //                  Console.WriteLine("The word {0} repeated: {1} times", item.Key, item.Value);
  //              }
  //          }
  //      }
  //
  //      [Benchmark(Baseline = true)]
  //      public ConcurrentDictionary<string, int> RunCountCharacterRxTest()
  //      {
  //          int init = Environment.TickCount;
  //          Task<ConcurrentDictionary<string, int>> task = new AsyncEnumerableIOBiggestWordStrategy().countWordsFromFileAsync(folderPath);
  //          task.Wait();
  //
  //          int elapsed = Environment.TickCount - init;
  //
  //          Console.WriteLine(@"Count all word RunCountCharacterRxTest took: {0} seconds", elapsed);
  //          //
  //          // Each benchmark should return a value to ensure the VM optimizations
  //          // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
  //          //
  //          return task.Result;
  //      }
  //
  //      [Benchmark]
  //      public int RunCharacterCountSyncTest()
  //      {
  //          char character = 'e';
  //          int init = Environment.TickCount;
  //          int ret = new EnumerableIOFindBiggestWordStrategy().findBiggestWordSync(folderPath);
  //
  //          int elapsed = Environment.TickCount - init;
  //
  //          Console.WriteLine(@"Count all wors RunCharacterCountSyncTest took: {0} seconds", elapsed);
  //
  //
  //          return ret;
  //      }
  //
  //
  //      [Benchmark]
  //      public int RunCharacterCountAsyncEnumerableTest()
  //      {
  //          char character = 'e';
  //          int init = Environment.TickCount;
  //          Task<int> task = new AsyncEnumerableIOBiggestWordStrategy().countCharactersFromFileAsync(folderPath, character);
  //          task.Wait();
  //          //
  //          // Each benchmark should return a value to ensure the VM optimizations
  //          // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
  //          //
  //          int elapsed = Environment.TickCount - init;
  //
  //          Console.WriteLine(@"Count all wors RunCharacterCountAsyncEnumerableTest took: {0} seconds", elapsed);
  //
  //
  //          return task.Result;
  //      }
  //
    }

}
