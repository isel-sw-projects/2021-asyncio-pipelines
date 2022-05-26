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
        public void RunRxTest()
        {
            FileReadStrategies.folderWordOccurrencesInSizeRangeWithRX(folderPath, minWordSize, maxWordSize).Wait();
        }
    }
}
