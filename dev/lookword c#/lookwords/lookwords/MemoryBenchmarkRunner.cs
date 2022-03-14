using BenchmarkDotNet.Attributes;
using lookwords.RxNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    [MemoryDiagnoser]
    public class MemoryBenchmarkRunner
    {
        static private string folderPath = @"F:\escola\MEIC\TESE\dev\lookwords-master\src\main\resources\gutenberg\";
        static ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
        static int minLength = 2;
        static int maxLength = 12;


        [Benchmark]
        public async void RunEnumerableTest()
        {

            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

            minLength = 2;
            maxLength = 12;

            ValueTask<ConcurrentDictionary<string, int>> enumePromise = FileUtils
                                                           .getLinesAsync(folderPath)
                                                           .Where(line => !line.Contains("*** END OF "))
                                                           .Select(line => new string(line.Where(c => !char.IsPunctuation(c)).ToArray()))
                                                           .Select(line => line.Split(' ')
                                                                               .Where(word => !string.IsNullOrEmpty(word))
                                                                               .Where(word => word.Length >= minLength && word.Length <= maxLength)
                                                                               )
                                                           .AggregateAsync(new ConcurrentDictionary<string, int>(), (prev, curr) =>
                                                           {
                                                               FileUtils.addWordToDictionary(curr, prev);
                                                               return prev;
                                                           });

            ConcurrentDictionary<string, int> words = await enumePromise;
           
            foreach (var curr in words)
            {
                Console.WriteLine("Palavra: " + curr.Key + " repetitions: " + curr.Value);
            }
        }


        [Benchmark]
        public void RunRxTest()
        {
            FolderWordsObservable observable = new FolderWordsObservable(folderPath);
            WordObserver obs = new WordObserver(words_dict, 2, 12);

            observable.Subscribe(obs);
        }
    }
}
