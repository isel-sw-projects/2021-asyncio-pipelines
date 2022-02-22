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
            Task<ConcurrentDictionary<String, int>> words = groupWordsAsyncEnum();

            words.Wait();

            foreach (var word in words.Result)
            {
                Console.WriteLine("Value: {0}, Count: {1}", word.Key, word.Value);
            }

            Console.ReadLine();
        }

        private static void RunRxTest()
        {
            ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
            FolderWordsObservable observable = new FolderWordsObservable(folderPath);
            WordObserver obs = new WordObserver(words_dict, 2, 12);

            observable.Subscribe(obs);

            foreach (var word in words_dict)
            {
                Console.WriteLine("Value: {0}, Count: {1}", word.Key, word.Value);
            }

            Console.ReadLine();
        }


        static async Task<ConcurrentDictionary<String, int>> groupWordsAsyncEnum()
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

        static void groupWordsRxNet()
        {

            int minLength = 2, maxLength= 12;
            
            FolderWordsObservable gw = new FolderWordsObservable(folderPath);
            ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();

            IObserver<string> observer = new WordObserver(words_dict, minLength, maxLength);

            gw.Subscribe(observer);


        }
    }
}
