using BenchmarkDotNet.Attributes;
using lookwords.BiggestWordFileReadStrategies.AsyncEnum;
using lookwords.BiggestWordFileReadStrategies.RxNet;
using lookwords.BiggestWordFileReadStrategies.SyncEnum;
using lookwords.noLinqStategies.SyncEnum;
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
  
           int elapsed = Environment.TickCount - init;
  
           Console.WriteLine(@"Count all word RunGetBiggestWordAsyncEnumerableTest took: {0} seconds", elapsed);
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
  
           int elapsed = Environment.TickCount - init;
  
           Console.WriteLine(@"Count all wors RunGetBiggestWordSyncTest took: {0} seconds", elapsed);
  
  
           return ret;
       }

        [Benchmark]
        public string RunGetBiggestWordWOLinqSyncTest()
        {

            int init = Environment.TickCount;
            string ret = new BiggestWordNoLinq().getBiggestWordInDirectory(folderPath);

            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Count all wors RunGetBiggestWordWOLinqSyncTest took: {0} seconds", elapsed);


            return ret;
        }



        [Benchmark]
       public string RunGetBiggestWordRxTest()
       {

           int init = Environment.TickCount;
           Task<string> task = new RXNetIOBiggestWordStrategy().getBiggestWordInDirectory(folderPath);
           task.Wait();
           //
           // Each benchmark should return a value to ensure the VM optimizations
           // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
           //
           int elapsed = Environment.TickCount - init;
  
           Console.WriteLine(@"Count all wors RunGetBiggestWordRxTest took: {0} seconds", elapsed);
  
  
           return task.Result;
       }
  
    }

}
