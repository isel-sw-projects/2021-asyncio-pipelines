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
    public class BenchmarckIOCountWordsStrategies
    {
        private string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");
        static int minWordSize = 5;
        static int maxWordSize = 10;

        public BenchmarckIOCountWordsStrategies(string folderPath = null)
        {
            if (folderPath != null)
            {
                this.folderPath = folderPath;
            }
        }

        public void RunSyncTest()
        {
            List<int> times = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                int init = Environment.TickCount;
                ConcurrentDictionary<string, int> result = new EnumerableIOFileReadStrategy().countWordsFromFileSync(folderPath, minWordSize, maxWordSize);
                int elapsed = Environment.TickCount - init;
                times.Add(elapsed);

                Console.WriteLine(@"RunSyncTest run {0} took: {1} miliseconds", i + 1, elapsed);

                if (i == 3)
                    PrintResults(result);
            }

            times.RemoveAt(0);

            double avg = times.Average();

            Console.WriteLine(@"Average time for RunSyncTest (last 3 runs): {0} miliseconds", avg);
        }

        public void RunCountWordsBaseline()
        {
            List<int> times = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                int init = Environment.TickCount;

                CountWordsBaseline test = new CountWordsBaseline();
                ConcurrentDictionary<string, int> result = test.countWordsFromFileASyncBaseline(folderPath, minWordSize, maxWordSize);

                int elapsed = Environment.TickCount - init;
                times.Add(elapsed);

                Console.WriteLine(@"RunCountWordsBaseline run {0} took: {1} miliseconds", i + 1, elapsed);

                if (i == 3)
                    PrintResults(result);
            }

            times.RemoveAt(0);

            double avg = times.Average();

            Console.WriteLine(@"Average time for RunCountWordsBaseline (last 3 runs): {0} miliseconds", avg);
        }

        public async Task RunAsyncEnumerableTest()
        {
            List<int> times = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                int init = Environment.TickCount;
                Task<ConcurrentDictionary<string, int>> task = new AsyncEnumerableIOFileReadStrategy().countWordsFromFileAsync(folderPath, minWordSize, maxWordSize);

                ConcurrentDictionary<string, int> result = await task;

                int elapsed = Environment.TickCount - init;
                times.Add(elapsed);

                Console.WriteLine(@"RunAsyncEnumerableTest run {0} took: {1} miliseconds", i + 1, elapsed);

                if (i == 3)
                    PrintResults(result);
            }

            times.RemoveAt(0);

            double avg = times.Average();

            Console.WriteLine(@"Average time for RunAsyncEnumerableTest (last 3 runs): {0} miliseconds", avg);
        }

        public async Task RunAsyncEnumerableWithToAsyncEnumerableConvertionTest()
        {
            List<int> times = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                int init = Environment.TickCount;
                Task<ConcurrentDictionary<string, int>> task = new AsyncEnumerableIOFileReadStrategy().countWordsFromFileAsync(folderPath, minWordSize, maxWordSize);

                ConcurrentDictionary<string, int> result = await task;

                int elapsed = Environment.TickCount - init;
                times.Add(elapsed);

                Console.WriteLine(@"RunAsyncEnumerableWithToAsyncEnumerableConvertionTest run {0} took: {1} miliseconds", i + 1, elapsed);

                if (i == 3)
                    PrintResults(result);
            }

            times.RemoveAt(0);

            double avg = times.Average();

            Console.WriteLine(@"Average time for RunAsyncEnumerableWithToAsyncEnumerableConvertionTest (last 3 runs): {0} miliseconds", avg);
        }

        public async Task RunRxTest()
        {
            List<int> times = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                int init = Environment.TickCount;
                Task<ConcurrentDictionary<string, int>> task = new RXNetIOFileReadStrategy().countWordsFromFileAsync(folderPath, minWordSize, maxWordSize);

                ConcurrentDictionary<string, int> result = await task;

                int elapsed = Environment.TickCount - init;
                times.Add(elapsed);

                Console.WriteLine(@"RunRxTest run {0} took: {1} miliseconds", i + 1, elapsed);

                if (i == 3)
                    PrintResults(result);
            }

            times.RemoveAt(0);

            double avg = times.Average();

            Console.WriteLine(@"Average time for RunRxTest (last 3 runs): {0} miliseconds", avg);
        }

        private void PrintResults(ConcurrentDictionary<string, int> results)
        {
            Console.WriteLine("\n\nResults:");
            foreach (var pair in results.OrderByDescending(pair => pair.Value).Take(10))
            {
                Console.WriteLine("Word: {0}, Count: {1}", pair.Key, pair.Value);
            }
            Console.WriteLine("\n\n");
        }
    }
}
