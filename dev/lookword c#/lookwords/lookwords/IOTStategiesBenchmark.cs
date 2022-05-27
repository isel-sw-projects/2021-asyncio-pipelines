using BenchmarkDotNet.Attributes;
using lookwords.RxNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace lookwords
{
    [MemoryDiagnoser]
    public class IOTStategiesBenchmark
    {
        static private string folderPath = @"F:\escola\MEIC\TESE\dev\lookwords-master\src\main\resources\gutenberg\";
        static ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
        static int minWordSize = 2;
        static int maxWordSize = 12;


        [Benchmark]
        public async void RunSyncTest()
        {
            FileReadStrategies.folderWordOccurrencesInSizeRangeSync(folderPath, minWordSize, maxWordSize);
        }

        //TODO TASK ONLY VERBOSE (W/O SYNTATIC SUGAR)

        //TODO proper testing, retrieve benchmar result

        [Benchmark]
        public async void RunEnumerableTest()
        {
            FileReadStrategies.folderWordOccurrencesInSizeRangeWithAsyncEnumerable(folderPath, minWordSize, maxWordSize).Wait();
        }
    


        [Benchmark]
        public ConcurrentDictionary<string, int> RunRxTest()
        {
            Task<ConcurrentDictionary<string, int>> task = FileReadStrategies.folderWordOccurrencesInSizeRangeWithRX(folderPath, minWordSize, maxWordSize);
            task.Wait();
            //
            // Each benchmark should return a value to ensure the VM optimizations
            // wil not discard the call to our operation, i.e. folderWordOccurrencesInSizeRangeWithRX
            //
            return task.Result;
        }
    }
}
