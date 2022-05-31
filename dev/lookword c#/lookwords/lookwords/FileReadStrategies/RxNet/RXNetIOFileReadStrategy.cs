using lookwords.RxNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.FileReadStrategies.RxNet
{
    public class RXNetIOFileReadStrategy : IIOFileReadStrategy
    {
      
        /// <summary>
        /// Collects all distinct words of given filePath into words Dictionary, which is shared across concurrent threads.
        /// ??? Not sure about the concurrent behavior of ForEachAsync ???
        /// </summary>
        private Task parseFileDistinctWordsIntoDictionary(string filePath, int minWordSize, int maxWordSize, ConcurrentDictionary<string, int> words)
        {
            return new FileTreeTextObservable(filePath)
                .Where(line => line.Length == 0)                           // Skip empty lines
                .Skip(14)                                                  // Skip gutenberg header
                .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                .SelectMany(line => line.Split(' ', ',', ';', '.', ':'))   // Split each line in Words
                .Where(word =>                                             // Filter words in range [minWordSize, maxWordSize]
                    word.Length >= minWordSize && word.Length <= maxWordSize)
                .ForEachAsync((word) => words.AddOrUpdate(word, 1, (k, v) => v + 1)); // Merge words in dictionary.
        }

        //RXNET implementation
        public Task<ConcurrentDictionary<string, int>> getDistingWordsFromFileAsync(string folderPath, int minWordSize, int maxWordSize)
        {
            Console.WriteLine("Initiation: ");
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

        ConcurrentDictionary<string, int> IIOFileReadStrategy.getDistingWordsFromFileSync(string folderName, int minWordLength, int maxWordLength)
        {
            throw new NotImplementedException();
        }
    }
}
