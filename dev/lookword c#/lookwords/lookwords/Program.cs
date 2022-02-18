using lookwords.fileUtils;
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
        static ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();
        static void Main(string[] args)
        {


            Task<Dictionary<String, int>> words = Run();

            words.Wait();

            foreach (var word in words.Result)
            {
                Console.WriteLine("Value: {0}, Count: {1}", word.Key, word.Value);
            }

            Console.ReadLine();
        }

        static async Task<Dictionary<String, int>> Run()
        {
            string folderPath = "F:\\escola\\MEIC\\TESE\\dev\\lookwords-master\\src\\main\resources\\gutenberg\\";
            Dictionary<String, int> words_dict = new Dictionary<String, int>();
            GroupWords gw = new GroupWords();

            IAsyncEnumerable<String> words = gw.Words(folderPath, 2, 10);

            await foreach (String word in words)
            {
                if(words_dict.ContainsKey(word))
                {
                    words_dict.Add(word, words_dict[word] + 1);
                } else
                {
                    words_dict.Add(word, 1);
                }
                
            }
            return words_dict;
        }
    }
}
