using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using lookwords.MixedSourceStrategies.FileReadStrategies.SyncEnum;
using lookwords.MixedSourceStrategies.FileReadStrategies.AsyncEnum;
using lookwords.MixedSourceStrategies.FileReadStrategies.RxNet;

namespace lookwords
{
    [MemoryDiagnoser]
    public class BenchmarckIOCountWordsStrategies
    {
        private string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");
        static int minWordSize = 1;
        static int maxWordSize = 8;

       public BenchmarckIOCountWordsStrategies(string folderPath = null)
       {
           if(folderPath != null)
           {
               this.folderPath = folderPath;
           }
       }


        [Benchmark(Baseline = true)]
        public ConcurrentDictionary<string, int> RunSyncTest()
        {
            int init = Environment.TickCount;
            ConcurrentDictionary<string, int> ret = new EnumerableIOFileReadStrategy().countWordsFromFileSync(folderPath, minWordSize, maxWordSize);

            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Count all words RunSyncTest took: {0} miliseconds", elapsed);

            return ret;
        }

        [Benchmark(Baseline = true)]
        public ConcurrentDictionary<string, int> RunCountWordsBaseline()
        {
            int init = Environment.TickCount;

            CountWordsBaseline test = new CountWordsBaseline();
            ConcurrentDictionary<string, int> ret = test.countWordsFromFileASyncBaseline(folderPath, 1, 8);


            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Count all words RunSyncTest without linq took: {0} miliseconds", elapsed);

            return ret;
        }


        [Benchmark]
        public ConcurrentDictionary<string, int> RunAsyncEnumerableTest()
        {
            int init = Environment.TickCount;
            Task<ConcurrentDictionary<string, int>> task = new AsyncEnumerableIOFileReadStrategy().countWordsFromFileAsync(folderPath, minWordSize, maxWordSize);
            task.Wait();
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Count all words RunAsyncEnumerableTest took: {0} miliseconds", elapsed);

           

            return task.Result;
        }

        [Benchmark]
        public ConcurrentDictionary<string, int> RunAsyncEnumerableWithToAsyncEnumerableConvertionTest()
        {
            int init = Environment.TickCount;
            Task<ConcurrentDictionary<string, int>> task = new AsyncEnumerableIOFileReadStrategy().countWordsFromFileAsync(folderPath, minWordSize, maxWordSize);
            task.Wait();
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Count all words RunAsyncEnumerableWithToAsyncEnumerableConvertionTest took: {0} miliseconds", elapsed);

           

            return task.Result;
        }

        [Benchmark]
        public ConcurrentDictionary<string, int> RunRxTest()
        {
            int init = Environment.TickCount;
            Task<ConcurrentDictionary<string, int>> task = new RXNetIOFileReadStrategy().countWordsFromFileAsync(folderPath, minWordSize, maxWordSize);
            task.Wait();

            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Count all words RxTest took: {0} miliseconds", elapsed);
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //

           
            return task.Result;
        }


        private void printResult(ConcurrentDictionary<string, int> dict)
        {
            foreach (var item in dict)
            {
                {
                    Console.WriteLine("The word {0} repeated: {1} times", item.Key, item.Value);
                }
            }
        }
    }
}
