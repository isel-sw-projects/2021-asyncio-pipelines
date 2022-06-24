using lookwords.RxNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lookwords.FileReadStrategies.RxNet
{
    public class RXNetIOFileReadStrategy 
    {
      
        /// <summary>
        /// Collects all distinct words of given filePath into words Dictionary, which is shared across concurrent threads.
        /// ??? Not sure about the concurrent behavior of ForEachAsync ???
        /// </summary>
        private Task parseFileDistinctWordsIntoDictionary(string filePath, int minWordSize, int maxWordSize, ConcurrentDictionary<string, int> words)
        {
            return new FileTreeTextObservable(filePath)
                .Where(line => line.Length != 0)                           // Skip empty lines
                .Skip(14)                                                  // Skip gutenberg header
                .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                .SelectMany(line => Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' '))
                .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize)
                .ForEachAsync((word) => { // Implicit Subscribe on Observable()
                    //Console.WriteLine(word);
                    words.AddOrUpdate(word, 1, (k, v) => v + 1);
                
                }); // Merge words in dictionary.
        }

        //RXNET implementation
        public Task<ConcurrentDictionary<string, int>> countWordsFromFileAsync(string folderPath, int minWordSize, int maxWordSize)
        {
            var words = new ConcurrentDictionary<string, int>();
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            List<Task> allTasks = Directory
                            .GetFiles(folderPath, "*.txt", SearchOption.AllDirectories)
                            .Select(file => parseFileDistinctWordsIntoDictionary(file, minWordSize, maxWordSize, words))
                            .ToList();
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return Task.WhenAll(allTasks).ContinueWith((prev) => words);
        }


    }
}
