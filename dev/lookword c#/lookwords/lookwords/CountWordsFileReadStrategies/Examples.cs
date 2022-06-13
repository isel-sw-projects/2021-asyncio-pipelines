using lookwords.RxNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    public class Examples
    {
        public static Dictionary<string, int> folderWordOccurrencesInSizeRangeSync(string folderPath, int minWordSize, int maxWordSize)
        {
            Dictionary<String, int> words_dict = new Dictionary<String, int>();
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);


            allfiles.ToList()
               .ForEach(async currFile =>
               {
                   IEnumerable<string> enume = FileUtils.getLinesSync(currFile, minWordSize, maxWordSize)
                           .TakeWhile(line => !line.Contains("*** END OF "))
                           .Select(line => new string(line.Where(c => !char.IsPunctuation(c)).ToArray()))
                           .SelectMany(line => line.Split(' ')
                                               .Where(word => !string.IsNullOrEmpty(word))
                                               .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize));
                   
                   
                   FileUtils.addWordToDictionary(enume, words_dict);
                           
               });

            //foreach (var curr in words_dict)
            //{
            //    Console.WriteLine("Palavra: " + curr.Key + " repetitions: " + curr.Value);
            //}
            return words_dict;
        }

        public static async Task<Dictionary<string, int>> folderWordOccurrencesInSizeRangeWithAsyncEnumerable(string folderPath, int minWordSize, int maxWordSize)
        {
            Dictionary<String, int> words_dict = new Dictionary<String, int>();
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);


            allfiles.ToList()
               .ForEach(async currFile =>
               {
                   await FileUtils.getLinesAsyncEnum(currFile, minWordSize, maxWordSize)
                            .TakeWhile(line => !line.Contains("*** END OF "))
                            .Select(line => new string(line.Where(c => !char.IsPunctuation(c)).ToArray()))
                            .Select(line => line.Split(' ')
                                                .Where(word => !string.IsNullOrEmpty(word))
                                                .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize))
                            .ForEachAsync(word => FileUtils.addWordToDictionary(word, words_dict));
               });

            foreach (var curr in words_dict)
            {
                Console.WriteLine("Palavra: " + curr.Key + " repetitions: " + curr.Value);
            }

            return words_dict;
        }

        /// <summary>
        /// Collects all distinct words of given filePath into words Dictionary, which is shared across concurrent threads.
        /// ??? Not sure about the concurrent behavior of ForEachAsync ???
        /// </summary>
        private static Task parseFileDistinctWordsIntoDictionary(string filePath, int minWordSize, int maxWordSize, ConcurrentDictionary<string, int> words) {
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
        public static Task<ConcurrentDictionary<string, int>> folderWordOccurrencesInSizeRangeWithRX(string folderPath, int minWordSize, int maxWordSize)
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
    }
}
