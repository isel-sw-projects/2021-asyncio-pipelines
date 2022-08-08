using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace lookwords.MixedSourceStrategies.FindWordStrategies.FindAsyncEnum
{
    public class AsyncEnumerableIOFindWordStrategy 
    {
        

        private Task<bool> findWordInFile(string filename, string word)
        {
            return FileUtils.getLinesAsyncEnum(filename)
                 .Where(line => line.Length != 0)                     // Skip empty lines
                 .Skip(14)                                            // Skip gutenberg header
                 .TakeWhile(line => !line.Contains("*** END OF "))    // Skip gutenberg footnote
                 .AnyAsync(line => {
                     if (line.Contains(word)) {
                            Console.WriteLine("found in: " + filename);
                            return true;
                        }
                        return false; 
                    })
                 .AsTask();
                 
               
        }


        public Task<bool> findWordInDirectory(string folderName, string word)
        {
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            Task<bool> allTasks = Directory
                           .GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                           .Select(file => findWordInFile(file, word))
                           .ToAsyncEnumerable()
                           .AnyAsync(tsk => tsk.Result)
                           .AsTask();
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return allTasks;
        }

    }
}
