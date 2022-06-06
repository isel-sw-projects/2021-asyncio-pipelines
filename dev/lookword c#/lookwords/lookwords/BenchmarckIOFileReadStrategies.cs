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
           return new EnumerableIOFileReadStrategy().getDistingWordsFromFileSync(folderPath, minWordSize, maxWordSize);
        }



        [Benchmark]
        public ConcurrentDictionary<string, int> RunAsyncEnumerableTest()
        {
            Task<ConcurrentDictionary<string, int>> task = new AsyncEnumerableIOFileReadStrategy().getDistingWordsFromFileAsync(folderPath, minWordSize, maxWordSize);
            task.Wait();
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            return task.Result;
        }

        [Benchmark]
        public ConcurrentDictionary<string, int> RunAsyncEnumerableWithToAsyncEnumerableConvertionTest()
        {
            Task<ConcurrentDictionary<string, int>> task = new AsyncEnumerableIOFileReadStrategyWithConverter().getDistingWordsFromFileAsync(folderPath, minWordSize, maxWordSize);
            task.Wait();
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            return task.Result;
        }

        [Benchmark]
        public ConcurrentDictionary<string, int> RunRxTest()
        {
            Task<ConcurrentDictionary<string, int>> task = new RXNetIOFileReadStrategy().getDistingWordsFromFileAsync(folderPath, minWordSize, maxWordSize);
            task.Wait();
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            return task.Result;
        }
    }
}
