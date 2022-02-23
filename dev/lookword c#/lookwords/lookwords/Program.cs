using lookwords.fileUtils;
using lookwords.RxNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    internal class Program
    {
        static private string folderPath = @"F:\escola\MEIC\TESE\dev\lookwords-master\src\main\resources\gutenberg\";
        static Boolean hasEnded = false;

        static void Main(string[] args)
        {

            RunRxTest();

        }


        private static void RunEnumerableTest()
        {
            Task<ConcurrentDictionary<String, int>> words = countWordsAsyncEnum();

            words.Wait();

            foreach (var word in words.Result)
            {
                Console.WriteLine("Value: {0}, Count: {1}", word.Key, word.Value);
            }

        }


        private static void RunRxTest()
        {
            ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
            FolderWordsObservable observable = new FolderWordsObservable(folderPath);
            WordObserver obs = new WordObserver(words_dict, 2, 12);

            observable.Subscribe(obs);

            
        }


        private static async Task<ConcurrentDictionary<String, int>> countWordsAsyncEnum()
        {
            FileWordsEnumerable gw = new FileWordsEnumerable();
            ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
            IAsyncEnumerable<String> words = gw.GetFolderWordsAsyncEnumerable(folderPath, 2, 10);

            await foreach (String word in words)
            {
                if (words_dict.ContainsKey(word))
                {
                    int nextValue;
                    while (!words_dict.TryRemove(word, out nextValue)) { };

                    while (!words_dict.TryAdd(word, nextValue + 1)) { };
                }
                else
                {
                    while (!words_dict.TryAdd(word, 1)) { };
                }

            }
            return words_dict;
        }

    }
}
