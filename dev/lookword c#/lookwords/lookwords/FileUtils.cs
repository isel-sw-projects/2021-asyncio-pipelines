
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    internal class FileUtils
    {

        public static async IAsyncEnumerable<String> getLinesAsync(String folderPath)
        {
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        yield return await reader.ReadLineAsync();
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

    }
}
