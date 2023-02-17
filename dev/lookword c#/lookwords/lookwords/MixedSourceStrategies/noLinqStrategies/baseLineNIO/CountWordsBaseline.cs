
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lookwords
{
  
    public class CountWordsBaseline 
    {

        private async Task parseFileDistinctWordsIntoDictionary(string filename, int minWordSize, int maxWordSize, ConcurrentDictionary<string, int> words)
        {
            //Console.WriteLine(filename);
            int count = 0;

            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        Task<string> lineTask = reader.ReadLineAsync();
                        lineTask.Wait();
                        string line = lineTask.Result;

                        if (count < 14 || line.Length == 0)
                        {
                            count++;
                            continue;
                        }
                        if (line.Contains("*** END OF "))
                        {
                            break;
                        }
                        string[] wordsInLine = Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' ');

                        foreach (string word in wordsInLine)
                        {
                            if (word.Length >= minWordSize && word.Length <= maxWordSize)
                            {
                                words.AddOrUpdate(word, 1, (k, v) => v + 1);
                            }
                        }
                    }
                }
            }
        }


        public ConcurrentDictionary<string, int> countWordsFromFileASyncBaseline(string folderName, int minWordLength, int maxWordLength)
        {
            var words = new ConcurrentDictionary<string, int>();
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                     .ToList()
                     .ForEach(file => {
                         parseFileDistinctWordsIntoDictionary(file, minWordLength, maxWordLength, words).Wait();
                         
                      });
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return words;
        }

    }
}
