using lookwords.FileReadStrategies;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lookwords.BiggestWordFileReadStrategies.SyncEnum
{
    public class EnumerableIOBiggestWordStrategy 
    {


        private void parseFileDistinctWordsIntoDictionary(string filename, int minWordSize, int maxWordSize, ConcurrentDictionary<string, int> words)
        {
           int wordLength = 0;

            FileUtils.getLinesSync(filename, minWordSize, maxWordSize)
                .Where(line => line.Length != 0)                           // Skip empty lines
                .Skip(14)                                                  // Skip gutenberg header
                .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                .SelectMany(line => Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' '))
                .Where(word => word.Length > wordLength)
                .ToList()
                .ForEach((word) => {
                    wordLength = word.Length;
                     //Console.WriteLine(word);
                    words.AddOrUpdate(word, 1, (k, v) => v + 1);
                }); // Merge words in dictionary.
        }


        public ConcurrentDictionary<string, int> countWordsFromFileSync(string folderName, int minWordLength, int maxWordLength)
        {
            var words = new ConcurrentDictionary<string, int>();
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                     .ToList()
                     .ForEach(file => parseFileDistinctWordsIntoDictionary(file, minWordLength, maxWordLength, words));
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return words;
        }


    }
    
}
