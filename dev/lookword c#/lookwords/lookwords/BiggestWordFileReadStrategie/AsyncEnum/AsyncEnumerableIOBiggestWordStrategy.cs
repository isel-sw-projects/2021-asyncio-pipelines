using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace lookwords.BiggestWordFileReadStrategies.AsyncEnum
{
    public class AsyncEnumerableIOBiggestWordStrategy 
    {
        

        private Task<string> parseFileDistinctWordsIntoDictionary(string filename)
        {
            return FileUtils.getLinesAsyncEnum(filename)
                 .Where(line => line.Length != 0)                     // Skip empty lines
                 .Skip(14)                                            // Skip gutenberg header
                 .TakeWhile(line => !line.Contains("*** END OF "))    // Skip gutenberg footnote
                 .Select(line => Regex.Replace(line, "[^a-zA-Z0- 9-]+", "", RegexOptions.Compiled).Split(' ')) //?? to review ?? 
                 .Select((arr) => arr.OrderByDescending(s => s.Length).First())
                 .FirstOrDefaultAsync()
                 .AsTask();
        }


        public Task<string> countWordsFromFileAsync(string folderName)
        {
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            List<Task<string>> allTasks = Directory
                            .GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                            .Select(file => parseFileDistinctWordsIntoDictionary(file))
                            .ToList();
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return Task.WhenAll(allTasks).ContinueWith(task => task.Result.OrderByDescending(s => s.Length).First());
        }

    }
}
