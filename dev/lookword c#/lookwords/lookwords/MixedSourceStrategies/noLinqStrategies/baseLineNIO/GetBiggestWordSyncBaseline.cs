using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lookwords.MixedSourceStrategies.noLinqStrategies.baseLineNIO
{
    internal class GetBiggestWordSyncBaseline
    {
         string getBiggestWordInFile(string filename)
        {
            int count = 0;
            string biggestWord = "";
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    //todo 
                    string line =  reader.ReadLine();


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


        public string getBiggestWordInDirectorySyncBaseline(string folderName)
        {
            string BiggestWord = "";
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //to do same that is done in java 
            //


            List<string> list = Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                     .ToList()
                     .Select(file => getBiggestWordInFile(file))
                     .ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Length > BiggestWord.Length)
                {
                    BiggestWord = list[i];
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
