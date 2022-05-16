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
    public class LookforWordsIOTests
    {
        static private string folderPath = @"F:\escola\MEIC\TESE\dev\lookwords-master\src\main\resources\gutenberg\";
        static ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
        static int minWordSize = 2;
        static int maxWordSize = 12;


        //TODO BLOCKING example

        //TODO TASK ONLY VERBOSE (W/O SYNTATIC SUGAR)

        //TODO proper testing, retrieve benchmar result

        [Benchmark]
        public async void RunEnumerableTest()
        {
            ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

            minWordSize = 2;
            maxWordSize = 12;

             allfiles.ToList()
                .ForEach(async currFile =>
                    {
                        await FileUtils.getLinesAsyncEnum(currFile, minWordSize, maxWordSize)
                                .TakeWhile(line => !line.Contains("*** END OF "))
                                .Select(line => new string(line.Where(c => !char.IsPunctuation(c)).ToArray()))
                                .Select(line => line.Split(' ')
                                                    .Where(word => !string.IsNullOrEmpty(word))
                                                    .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize)
                                                    .ToList())
                                .ForEachAsync(word => FileUtils.addWordToDictionary(word, words_dict));
                    });

            foreach (var curr in words_dict)
            {
                Console.WriteLine("Palavra: " + curr.Key + " repetitions: " + curr.Value);
            }
        }
    


        [Benchmark]
        public async void RunRxTest()
        {
            Console.WriteLine("Initiation: ");
            ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {


                FolderWordsObservable observable = new FolderWordsObservable(folderPath);
                WordObserver obs = new WordObserver(words_dict, 2, 12);

                observable.Subscribe(obs);
                await observable.TakeWhile(line => !line.Contains("*** END OF "))
                           .Select(line => new string(line.Where(c => !char.IsPunctuation(c)).ToArray()))
                           .Select(line => line.Split(' ')
                                             .Where(word => !string.IsNullOrEmpty(word))
                                             .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize)
                                             .ToList())
                           .ForEachAsync(word => FileUtils.addWordToDictionary(word, words_dict));

            }

            foreach (var curr in words_dict)
            {
                Console.WriteLine("Palavra: " + curr.Key + " repetitions: " + curr.Value);
            }
        }
    }
}
