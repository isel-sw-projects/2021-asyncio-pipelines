using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lookwords.MixedSourceStrategies.FileReadStrategies.AsyncEnum
{
    public class AsyncEnumerableIOFileReadStrategy 
    {
        

        private  Task parseFileDistinctWordsIntoDictionary(string filename, int minWordSize, int maxWordSize, ConcurrentDictionary<string, int> words)
        {
            return FileUtils.getLinesAsyncEnum(filename)
                 .Where(line => line.Length != 0)                     // Skip empty lines
                 .Skip(14)                                            // Skip gutenberg header
                 .TakeWhile(line => !line.Contains("*** END OF "))    // Skip gutenberg footnote
                 .Select(line => Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' ')) //?? to review ?? 
                 .ForEachAsync((arr) => arr
                    .Where(word => word.Length > minWordSize && word.Length < maxWordSize)
                    .Aggregate(words, (prev, word) =>
                    {
                        prev.AddOrUpdate(word, 1, (k, v) => v + 1); // Merge words in dictionary.
                        return prev;
                    })
                );     
        }


        public Task<ConcurrentDictionary<string, int>> countWordsFromFileAsync(string folderName, int minWordLength, int maxWordLength)
        {
            var words = new ConcurrentDictionary<string, int>();
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            List<Task> allTasks = Directory
                            .GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                            .Select(file => parseFileDistinctWordsIntoDictionary(file, minWordLength, maxWordLength, words))
                            .ToList();
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return Task.WhenAll(allTasks).ContinueWith((prev) => words);
        }

     
    }
}
