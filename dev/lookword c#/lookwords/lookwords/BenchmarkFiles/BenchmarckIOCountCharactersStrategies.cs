using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using lookwords.FileReadStrategies.AsyncEnum;
using lookwords.FileReadStrategies.RxNet;
using lookwords.FileReadStrategies.SyncEnum;
using lookwords.CountCharactersStrategies.AsyncEnum;
using lookwords.CountCharactersStrategies.SyncEnum;

namespace lookwords
{
    [MemoryDiagnoser]
    public class BenchmarckIOCountCharactersStrategies
    {
        static private string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");
        static ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
        static int minWordSize = 2;
        static int maxWordSize = 8;


        private void printResult(ConcurrentDictionary<string, int> dict)
        {
            foreach (var item in dict)
            {
                {
                    Console.WriteLine("The word {0} repeated: {1} times", item.Key, item.Value);
                }
            }
        }

        [Benchmark(Baseline = true)]
        public ConcurrentDictionary<string, int> RunCountCharacterRxTest()
        {
            int init = Environment.TickCount;
            Task<ConcurrentDictionary<string, int>> task = new RXNetIOFileReadStrategy().countWordsFromFileAsync(folderPath, minWordSize, maxWordSize);
            task.Wait();
          
            int elapsed = Environment.TickCount - init;
          
            Console.WriteLine(@"Count all word RunCountCharacterRxTest took: {0} seconds", elapsed);
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            return task.Result;
        }
          
        [Benchmark]
        public int RunCharacterCountSyncTest()
        {
            char character = 'e';
            int init = Environment.TickCount;
            int ret = new EnumerableIOCountCharacterStrategy().countCharactersFromFileSync(folderPath, character);
          
            int elapsed = Environment.TickCount - init;
          
            Console.WriteLine(@"Count all wors RunCharacterCountSyncTest took: {0} seconds", elapsed);
          
          
            return ret;
        }
          
          
        [Benchmark]
        public int RunCharacterCountAsyncEnumerableTest()
        {
            char character = 'e';
            int init = Environment.TickCount;
            Task<int> task = new AsyncEnumerableIOBiggestWordStrategy().countCharactersFromFileAsync(folderPath, character);
            task.Wait();
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            int elapsed = Environment.TickCount - init;
          
            Console.WriteLine(@"Count all wors RunCharacterCountAsyncEnumerableTest took: {0} seconds", elapsed);
          
          
            return task.Result;
        }
        
    }
}
