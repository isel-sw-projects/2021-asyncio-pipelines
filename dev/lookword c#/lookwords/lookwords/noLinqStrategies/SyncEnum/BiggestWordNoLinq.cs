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
        private string getBiggestWordInFile(string filename)
        {
            IEnumerable lines = FileUtils.getLinesSync(filename);
            int count = 0;
            string biggestWord = "";
            foreach (string line in lines)
            {

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


        public string getBiggestWordInDirectory(string folderName)
        {
            string BiggestWord = "";
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                     .ToList()
                     .ForEach(file =>
                     {
                         string curr = getBiggestWordInFile(file);
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
