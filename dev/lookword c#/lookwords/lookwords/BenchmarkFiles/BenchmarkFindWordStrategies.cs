using BenchmarkDotNet.Attributes;
using lookwords.MixedSourceStrategies.FindWordStrategies.FindAsyncEnum;
using lookwords.MixedSourceStrategies.FindWordStrategies.RxNet;
using lookwords.MixedSourceStrategies.FindWordStrategies.SyncEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.BenchmarkFiles
{
    public class BenchmarkFindWordStrategies
    {
        static private string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");

        [Benchmark]
        public bool RunGetFindWordSyncTest(string word)
        {

            int init = Environment.TickCount;
            bool ret = new EnumerableIOFindWordStrategy().findWordInDirectory(folderPath, word);


            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find word sync test took: {0} seconds", elapsed);


            return ret;
        }

        [Benchmark]
        public bool RunFindWordAsyncTest(string word)
        {

            int init = Environment.TickCount;
            Task<bool> ret = new AsyncEnumerableIOFindWordStrategy().findWordInDirectory(folderPath, word);
            ret.Wait();
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find word async test took: {0} seconds", elapsed);


            return ret.Result;
        }



        [Benchmark]
        public bool RunFindWorddRxTest(string word)
        {

            int init = Environment.TickCount;
            IObservable<bool> obsrvable = new RXNetIOFindWordStrategy().findWordInDirectory(folderPath, word);

            var t = new TaskCompletionSource<bool>();
            obsrvable.Subscribe(str => t.TrySetResult(str));

            t.Task.Wait();

            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Find word RX TEST took: {0} seconds", elapsed);


            return t.Task.Result;
        }
    }
}
