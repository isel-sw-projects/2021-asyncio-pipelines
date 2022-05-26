
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    public class FileUtils
    {

        public static IEnumerable<String> getLinesSync(String file, int minSize, int maxSize)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                   yield return reader.ReadLine();
                }
            }

        }

        public static async IAsyncEnumerable<String> getLinesAsyncEnum(String file, int minSize, int maxSize)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    yield return await reader.ReadLineAsync();
                }
            }
            
        }

        public static void addWordToDictionary(IEnumerable<string> line, Dictionary<String, int> dict)
        {
            foreach (string word in line)
            {
                Console.WriteLine(word);//to comment
                if (dict.ContainsKey(word))
                {
                    dict[word] = ++dict[word];
                }
                else
                {
                    dict.Add(word, 1);
                }
            }
        }
    }
}
