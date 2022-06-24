using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.CountCharactersStrategies.RxNet
{
    public class RXNetIOBiggestWordStrategy 
    {
      
        /// <summary>
        /// Collects all distinct words of given filePath into words Dictionary, which is shared across concurrent threads.
        /// ??? Not sure about the concurrent behavior of ForEachAsync ???
        /// </summary>
        private Task<int> countCharacterOcurrencesInFile(string filePath, char character)
        {
            return new FileTreeCharacterObservable(filePath)
                 .Where(charr => character == charr)                     // Skip empty lines
                 .ToAsyncEnumerable()
                 .CountAsync()
                 .AsTask();
                 
        }

       
        public Task<int> countCharactersFromFileAsync(string folderPath, char character)
        {

            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            List<Task<int>> allTasks = Directory
                            .GetFiles(folderPath, "*.txt", SearchOption.AllDirectories)
                            .Select(file => countCharacterOcurrencesInFile(file, character))
                            .ToList();
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return Task.WhenAll(allTasks)
                .ContinueWith(prev =>
                {
                    int count = 0;
                    return prev.Result.Aggregate(count, (prev, curr) => prev + curr);
                });

        }
    }
}
