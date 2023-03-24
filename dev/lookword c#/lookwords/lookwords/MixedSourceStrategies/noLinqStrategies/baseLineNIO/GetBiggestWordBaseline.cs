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
    public class GetBiggestWordBaseline
    {
        private Task<string> getBiggestWordInFile(string filename)
        {
            return Task.Factory.StartNew(() =>
            {

                int count = 0;
                string biggestWord = "";
                ConcurrentStack<Task<string>> lineTasksStack = new ConcurrentStack<Task<string>>();


                using (StreamReader reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                       
                        Task<string> curr = reader.ReadLineAsync().ContinueWith(tsk =>
                        {
                            string line;

                            line = tsk.Result;

                            if (count < 14 || line.Length == 0)
                            {
                                count++;
                                return null;
                            }

                            string[] wordsInLine = Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' ');

                            foreach (string word in wordsInLine)
                            {
                                if (word.Length >= biggestWord.Length)
                                {
                                    biggestWord = word;
                                }
                            }
                            return biggestWord;

                        });

                            lineTasksStack.Push(curr);
                    }
                    
                }

                string biggest = "";
                bool shouldIgnore = false;
                for (int i = 0; i < lineTasksStack.Count; i++)
                {

                    string curr = lineTasksStack.ElementAt(i).Result;

                    if(curr.Contains("*** END OF "))
                    {
                        shouldIgnore = true;
                    }

                    if(curr == null || shouldIgnore)
                    {
                        continue;
                    }

                    if (curr.Length > biggest.Length)
                    {
                        biggest = curr;
                    }
                }
                return biggest;
            });
           
    }


        public string getBiggestWordInDirectoryAsyncBaseline(string folderName)
        {
            string BiggestWord = "";
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //to do same that is done in java 
            //


            List<Task<string>> list = Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                     .ToList()
                     .Select(file =>getBiggestWordInFile(file))
                     .ToList();

            for(int i = 0; i < list.Count; i++)
            {
                if(list[i].Result.Length > BiggestWord.Length)
                {
                    BiggestWord = list[i].Result;
                }
            }

            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            Console.WriteLine("Biggest word is: {0}", BiggestWord);
            return BiggestWord;
        }
    }
}
