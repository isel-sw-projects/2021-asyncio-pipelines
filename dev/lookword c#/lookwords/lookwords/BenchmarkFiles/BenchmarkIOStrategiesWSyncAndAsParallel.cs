using BenchmarkDotNet.Attributes;
using lookwords.ParallelButWSyncSourceStrategies.BiggestWordFileReadStrategies.AsyncEnum;
using lookwords.ParallelButWSyncSourceStrategies.BiggestWordFileReadStrategies.RxNet;
using lookwords.ParallelButWSyncSourceStrategies.CountWords.RxNet;
using lookwords.ParallelButWSyncSourceStrategies.CoutnWords.AsyncEnum;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.BenchmarkFiles
{
    public class BenchmarkIOStrategiesWSyncAndAsParallel
    {

        private string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");
        static int minWordSize = 2;
        static int maxWordSize = 8;

        public BenchmarkIOStrategiesWSyncAndAsParallel(string folderPath = null)
        {
            if (folderPath != null)
            {
                this.folderPath = folderPath;
            }
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

            Console.WriteLine(@"Count all words RunAsyncEnumerableTest (SYNC SOURCE as Parallel) took: {0} miliseconds", elapsed);



            return task.Result;
        }



      //  [Benchmark]
      //  public ConcurrentDictionary<string, int> RunRxTest()
      //  {
      //      int init = Environment.TickCount;
      //      Task<ConcurrentDictionary<string, int>> task = new RXNetIOFileReadStrategy().countWordsFromFileAsync(folderPath, minWordSize, maxWordSize);
      //      task.Wait();
      //
      //      int elapsed = Environment.TickCount - init;
      //
      //      Console.WriteLine(@"Count all words RxTest (SYNC SOURCE) took: {0} miliseconds", elapsed);
      //      //
      //      // Each benchmark should return a value to ensure the VM optimizations
      //      // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
      //      //
      //
      //
      //      return task.Result;
      //  }



        [Benchmark(Baseline = true)]
        public string RunGetBiggestWordAsyncEnumerableTest()
        {
            int init = Environment.TickCount;
            Task<string> task = new AsyncEnumerableIOBiggestWordStrategy().getBiggestWordInDirectory(folderPath);
            task.Wait();

            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Count all words RunGetBiggestWordAsyncEnumerableTest (SYNC SOURCE as PARALLEL) took: {0} seconds", elapsed);
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            return task.Result;
        }



       // [Benchmark]
       // public string RunGetBiggestWordRxTest()
       // {
       //
       //     int init = Environment.TickCount;
       //     IObservable<string> obsrvable = new RXNetIOBiggestWordStrategy().getBiggestWordInDirectory(folderPath);
       //
       //     var t = new TaskCompletionSource<string>();
       //     obsrvable.Subscribe(str => t.TrySetResult(str));
       //
       //     t.Task.Wait();
       //
       //     //
       //     // Each benchmark should return a value to ensure the VM optimizations
       //     // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
       //     //
       //     int elapsed = Environment.TickCount - init;
       //
       //     Console.WriteLine(@"Count all words RunGetBiggestWordRxTest (SYNC SOURCE) took: {0} seconds", elapsed);
       //
       //
       //     return t.Task.Result;
       // }
    }
    
}
