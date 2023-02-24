using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lookwords.noLinqStategies.SyncEnum
{
    public class BiggestWordNoLinq
    {
        private async Task<string> getBiggestWordInFile(string filename)
        {
            int count = 0;
            string biggestWord = "";
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    Task<string> wordTask = reader.ReadLineAsync();
                    wordTask.Wait();
                    string line =  wordTask.Result;

                    if (count < 14 || line.Length == 0)
                    {
                        count++;
                        continue;
                    }
                    if (line.Contains("*** END OF "))
                    {
                        break;
                    }
                    string[] wordsInLine = Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' ');

                    foreach (string word in wordsInLine)
                    {
                        if (word.Length >= biggestWord.Length)
                        {
                            biggestWord = word;
                        }
                    }
                }
                return biggestWord;
            }
        }


        public string getBiggestWordInDirectoryAsyncBaseline(string folderName)
        {
            string BiggestWord = "";
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                     .ToList()
                     .ForEach(file =>
                     {
                         Task<string> biggestWord_task = getBiggestWordInFile(file);
                         string curr = biggestWord_task.Result;
                         if (curr.Length > BiggestWord.Length)
                         {
                             BiggestWord = curr;
                         }
                     });
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return BiggestWord;
        }
    }
}
