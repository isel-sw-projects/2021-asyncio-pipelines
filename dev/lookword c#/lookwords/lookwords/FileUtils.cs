
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

        public static async Task getWordsInSizeInterval (String folderPath, int minWordSize, int maxSize)
        {
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = await reader.ReadLineAsync();

                        if (line.Contains("*** END OF "))
                        {
                            return;
                        }
                        line = line.Trim();
                        IEnumerable<string> enume = line.Split(' ')
                            .Where(word => !string.IsNullOrEmpty(word))
                            .Where(word => word.Length >= minWordSize && word.Length <= maxSize);


                        foreach (string word in enume)
                        {
                            Console.WriteLine(word);
                        }
                    }
                }
            }
        }

        public static void addWordToDictionary(IEnumerable<string> line, ConcurrentDictionary<String, int> dict)
        {
            foreach (string word in line)
            {

                if (dict.ContainsKey(word))
                {
                    int nextValue = dict[word];
                    while (!dict.TryRemove(word, out nextValue)) { };

                    while (!dict.TryAdd(word, nextValue + 1)) { };
                }
                else
                {
                    while (!dict.TryAdd(word, 1)) { };
                }
            }
        }


        public static void proccessLine(string line, int minLength, int maxLength)
        {
           if( line.Contains("*** END OF "))
            {
                return;
            }
            line = line.Trim();
            IEnumerable<string> enume = line.Split(' ')
                .Where(word => !string.IsNullOrEmpty(word))
                .Where(word => word.Length >= minLength && word.Length <= maxLength);
                 

            foreach(string word in enume)
            {
                Console.WriteLine(word);
            }
        }
    }
}
