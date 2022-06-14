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
    public class BenchmarckIOFileReadStrategies
    {
        static private string folderPath = @"F:\escola\MEIC\TESE\dev\lookwords-master\src\main\resources\gutenberg\";
        static ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
        static int minWordSize = 2;
        static int maxWordSize = 8;


        [Benchmark(Baseline=true)]
        public ConcurrentDictionary<string,int> RunSyncTest()
        {
            int init = Environment.TickCount;
            ConcurrentDictionary<string,int> ret = new EnumerableIOFileReadStrategy().countWordsFromFileSync(folderPath, minWordSize, maxWordSize);

            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Count all wors RunSyncTest took: {0} seconds", elapsed);


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

            Console.WriteLine(@"Count all wors RunAsyncEnumerableTest took: {0} seconds", elapsed);


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

            Console.WriteLine(@"Count all word RunAsyncEnumerableWithToAsyncEnumerableConvertionTest took: {0} seconds", elapsed);

            return task.Result;
        }

        [Benchmark]
        public ConcurrentDictionary<string, int> RunRxTest()
        {
            int init = Environment.TickCount;
            Task<ConcurrentDictionary<string, int>> task = new RXNetIOFileReadStrategy().countWordsFromFileAsync(folderPath, minWordSize, maxWordSize);
            task.Wait();

            int elapsed = Environment.TickCount - init;

            Console.WriteLine(@"Count all word RxTest took: {0} seconds", elapsed);
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            return task.Result;
        }


        [Benchmark]
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
            Task<int> task = new AsyncEnumerableIOCountCharacterStrategy().countCharactersFromFileAsync(folderPath, character);
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
