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

namespace lookwords.BiggestWordFileReadStrategies.RxNet
{
    public class RXNetIOBiggestWordStrategy
    {

        /// <summary>
        /// Collects all distinct words of given filePath into words Dictionary, which is shared across concurrent threads.
        /// ??? Not sure about the concurrent behavior of ForEachAsync ???
        /// </summary>
        private Task<string> findBiggestWordInFile(string filePath)
        {
            string biggestWord = "";

            return new FileTreeTextObservable(filePath)
                .Where(line => line.Length != 0)                           // Skip empty lines
                .Skip(14)                                                  // Skip gutenberg header
                .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                .Select(line => Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled)
                                        .Split(' ')
                                        .Max(arr => arr))
                .ToAsyncEnumerable()
                .AggregateAsync("", (biggest, curr) => curr.Length > biggest.Length ? curr : biggest)
                .AsTask();


        }

        //RXNET implementation
        public Task<string> getBiggestWordInDirectory(string folderPath)
        {
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            List<Task<string>> allTasks = Directory
                            .GetFiles(folderPath, "*.txt", SearchOption.AllDirectories)
                            .Select(file => findBiggestWordInFile(file))
                            .ToList();
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return Task.WhenAll(allTasks).ContinueWith(task => task.Result.Aggregate("", (biggest, curr) => curr.Length > biggest.Length ? curr : biggest));
        }
    }
}
