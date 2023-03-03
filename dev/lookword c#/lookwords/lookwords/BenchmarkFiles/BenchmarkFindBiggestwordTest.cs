using BenchmarkDotNet.Attributes;
using lookwords.MixedSourceStrategies.BiggestWordFileReadStrategies.AsyncEnum;
using lookwords.MixedSourceStrategies.BiggestWordFileReadStrategies.SyncEnum;
using lookwords.noLinqStategies.SyncEnum;
using lookwords.SyncSourceStrategies.BiggestWordFileReadStrategies.RxNet;
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
        

  
       [Benchmark(Baseline = true)]
       public string RunGetBiggestWordAsyncEnumerableTest()
       {
           int init = Environment.TickCount;
           Task<string> task = new AsyncEnumerableIOBiggestWordStrategy().getBiggestWordInDirectory(folderPath);
            task.Wait();
            //Console.WriteLine("Biggest word is: {0}", task.Result);
            int elapsed = Environment.TickCount - init;
  
           Console.WriteLine(@"Find the biggest word with RunGetBiggestWordAsyncEnumerableTest took: {0} seconds", elapsed);
           //
           // Each benchmark should return a value to ensure the VM optimizations
           // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
           //
           return task.Result;
       }
  
       [Benchmark]
       public string RunGetBiggestWordSyncTest()
       {

           int init = Environment.TickCount;
           string ret = new EnumerableIOFindBiggestWordStrategy().getBiggestWordInDirectory(folderPath);

           // Console.WriteLine("Biggest word is: {0}", ret);
            int elapsed = Environment.TickCount - init;
  
           Console.WriteLine(@"Find the biggest word with RunGetBiggestWordSyncTest took: {0} seconds", elapsed);
  
  
           return ret;
       }

        [Benchmark]
        public string RunGetBiggestWordBaselineTest()
        {

            int init = Environment.TickCount;
            string ret = new GetBiggestWordBaseline().getBiggestWordInDirectoryAsyncBaseline(folderPath);

            //Console.WriteLine("Biggest word is: {0}", ret);
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find the biggest word with RunGetBiggestWordCSHARPBaseline took: {0} seconds", elapsed);


            return ret;
        }



        [Benchmark]
       public string RunGetBiggestWordRxTest()
       {

           int init = Environment.TickCount;
           IObservable<string> obsrvable = new RXNetIOBiggestWordStrategy().getBiggestWordInDirectory(folderPath);

            var t = new TaskCompletionSource<string>();
            obsrvable.Subscribe(str => t.TrySetResult(str));

            t.Task.Wait();
            
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            int elapsed = Environment.TickCount - init;
  
           Console.WriteLine(@"Find the biggest word with RunGetBiggestWordRxTest took: {0} seconds", elapsed);
  
  
           return t.Task.Result;
       }
  
    }

}
