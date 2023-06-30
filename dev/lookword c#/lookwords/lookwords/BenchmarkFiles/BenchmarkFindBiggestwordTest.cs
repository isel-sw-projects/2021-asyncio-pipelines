using BenchmarkDotNet.Attributes;
using lookwords.MixedSourceStrategies.BiggestWordFileReadStrategies.AsyncEnum;
using lookwords.MixedSourceStrategies.BiggestWordFileReadStrategies.SyncEnum;
using lookwords.MixedSourceStrategies.noLinqStrategies.baseLineNIO;
using lookwords.MixedSourceStrategies.noLinqStrategies.baseLineNIO.lookwords.noLinqStategies.SyncEnum;
using lookwords.noLinqStategies.SyncEnum;
using lookwords.noLinqStategies.SyncEnum.lookwords.noLinqStategies.SyncEnum;
using lookwords.SyncSourceStrategies.BiggestWordFileReadStrategies.RxNet;
using lookwords.SyncSourceStrategies.FindBiggestWordFileReadStrategies;
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
            Console.WriteLine("Biggest word is: {0}", task.Result);
            int elapsed = Environment.TickCount - init;
  
           Console.WriteLine(@"Find the biggest word with RunGetBiggestWordAsyncEnumerableTest took: {0} seconds", elapsed);
           //
           // Each benchmark should return a value to ensure the VM optimizations
           // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
           //
           return task.Result;
       }
  
       [Benchmark]
       public string RunGetBiggestWordLinqSyncTest()
       {

           int init = Environment.TickCount;
           string ret = new EnumerableLinqFindBiggestWordStrategy().getBiggestWordInDirectory(folderPath);

           // Console.WriteLine("Biggest word is: {0}", ret);
            int elapsed = Environment.TickCount - init;
  
           Console.WriteLine(@"Find the biggest word with RunGetBiggestWordLinqSyncTest took: {0} seconds", elapsed);
  
  
           return ret;
       }

        [Benchmark]
        public string RunGetBiggestWordBaselineTest()
        {

            int init = Environment.TickCount;
            string ret = new GetBiggestWordSyncBaseline().getBiggestWordInDirectorySyncBaseline(folderPath);

            Console.WriteLine("Biggest word is: {0}", ret);
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find the biggest word with getBiggestWordInDirectorySyncBaseline took: {0} seconds", elapsed);


            return ret;
        }

        [Benchmark]
        public string RunGetBiggestWordAsyncBaselineTest()
        {

            int init = Environment.TickCount;
            string ret = new GetBiggestWordBaselineAsync().getBiggestWordInDirectoryAsyncBaseline(folderPath).Result;

            Console.WriteLine("Biggest word is: {0}", ret);
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find the biggest word with getBiggestWordInDirectoryAsyncBaseline took: {0} seconds", elapsed);


            return ret;
        }

        [Benchmark]
        public string RunGetBiggestWordAsyncBaseline()
        {

            int init = Environment.TickCount;
            string ret = new GetBiggestWordBaselin().getBiggestWordAsyncBaseline_1(folderPath).Result;

            Console.WriteLine("Biggest word is: {0}", ret);
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find the biggest word with GetBiggestWordAsyncBaseline took: {0} seconds", elapsed);


            return ret;
        }

        [Benchmark]
        public string RunBiggestWordBaselineTaskMultithreadBlockingRead()
        {

            int init = Environment.TickCount;
            string ret = new GetBiggestWordBaselineAsyncBlockingRead().getBiggestWordInDirectoryBaseline(folderPath).Result;

            Console.WriteLine("Biggest word is: {0}", ret);
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find the biggest word with GetBiggestWordBaselineMultithreadBlockingRead took: {0} seconds", elapsed);


            return ret;
        }


        [Benchmark]
        public string RunGetBiggestWordBaselineTestNoContinueWith()
        {

            int init = Environment.TickCount;
            string ret = new GetBiggestWordBaselineNoContinueWith().getBiggestWordInDirectoryAsyncBaseline(folderPath);

            //Console.WriteLine("Biggest word is: {0}", ret);
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find the biggest word with getBiggestWordInDirectoryAsyncBaseline took: {0} seconds", elapsed);


            return ret;
        }

        [Benchmark]
        public string RunGetBiggestWordSyncBaselineTest()
        {

            int init = Environment.TickCount;
            string ret = new GetBiggestWordSyncBaseline().getBiggestWordInDirectorySyncBaseline(folderPath);

            //Console.WriteLine("Biggest word is: {0}", ret);
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find the biggest word with getBiggestWordInDirectorySyncBaseline took: {0} seconds", elapsed);


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

            Console.WriteLine("Biggest word is: {0}", t.Task.Result);

            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            int elapsed = Environment.TickCount - init;
  
           Console.WriteLine(@"Find the biggest word with RunGetBiggestWordRxTest took: {0} seconds", elapsed);
  
  
           return t.Task.Result;
       }

        [Benchmark]
        public string RunGetBiggestWordRxAsyncFileReadTest()
        {

            int init = Environment.TickCount;
            IObservable<string> obsrvable = new RXAsyncStrategy().getBiggestWordInDirectoryAsync(folderPath);

            var t = new TaskCompletionSource<string>();
            obsrvable.Subscribe(str => t.TrySetResult(str));

            t.Task.Wait();

            Console.WriteLine("Biggest word is: {0}", t.Task.Result);

            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find the biggest word with RunGetBiggestWordRxAsyncFileReadTest took: {0} seconds", elapsed);


            return t.Task.Result;
        }

    }

}
