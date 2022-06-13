using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.CountCharactersStrategies.AsyncEnum
{
    public class AsyncEnumerableIOCountCharacterStrategy : IIOFileReadStrategy
    {
        

        private  Task<int> getCharacterOcurrencesInFile(string filename, char charactr)
        {

            return FileUtils.getCharacterAsyncEnum(filename)
                 .Where(charr => charactr == charr)                     // Skip empty lines
                 .CountAsync()
                 .AsTask();   
        }


        public Task<ConcurrentDictionary<string, int>> countWordsFromFileAsync(string folderName, int minWordLength, int maxWordLength)
        {
            throw new NotImplementedException();
        }

        public ConcurrentDictionary<string, int> countWordsFromFileSync(string folderName, int minWordLength, int maxWordLength)
        {
            throw new NotImplementedException();
        }

        Task<int> IIOFileReadStrategy.countCharactersFromFileAsync(string folderName, char character)
        {
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            List<Task<int>> allTasks = Directory
                            .GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                            .Select(file => getCharacterOcurrencesInFile(file, character))
                            .ToList();
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return Task.WhenAll(allTasks)
                .ContinueWith(prev => {
                    int count = 0;
                    return prev.Result.Aggregate(count, (prev, curr) => prev+curr);
                });
        }

        public int countCharactersFromFileSync(string folderName, char character)
        {
            throw new NotImplementedException();
        }
    }
}
