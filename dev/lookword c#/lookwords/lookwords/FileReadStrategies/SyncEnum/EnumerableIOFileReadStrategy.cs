using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.FileReadStrategies.SyncEnum
{
    public class EnumerableIOFileReadStrategy : IIOFileReadStrategy
    {
        private void parseFileDistinctWordsIntoDictionary(string filename, int minWordSize, int maxWordSize, ConcurrentDictionary<string, int> words)
        {
             FileUtils.getLinesSync(filename, minWordSize, maxWordSize)
                 .Where(line => line.Length == 0)                           // Skip empty lines
                 .Skip(14)                                                  // Skip gutenberg header
                 .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                 .SelectMany(line => line.Split(' ', ',', ';', '.', ':')) //?? to review ?? 
                 .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize)
                 .ToList()
                 .ForEach((word) => words.AddOrUpdate(word, 1, (k, v) => v + 1)); // Merge words in dictionary.
        }


        public ConcurrentDictionary<string, int> getDistingWordsFromFileSync(string folderName, int minWordLength, int maxWordLength)
        {

            Console.WriteLine("Initiation: ");
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

        public Task<ConcurrentDictionary<string, int>> getDistingWordsFromFileAsyncEnumerable(string folderName, int minWordLength, int maxWordLength)
        {
            throw new NotImplementedException();
        }
    }
}
