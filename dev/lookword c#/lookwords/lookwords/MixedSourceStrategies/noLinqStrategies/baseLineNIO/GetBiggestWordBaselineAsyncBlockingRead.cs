using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.MixedSourceStrategies.noLinqStrategies.baseLineNIO
{
     namespace lookwords.noLinqStategies.SyncEnum
     {
         public class GetBiggestWordBaselineAsyncBlockingRead
         {
             private Task<String> GetBiggestWordInFileAsync(string filename)
             {
                return Task.Run(() =>
                {
                    string biggestWord = "";

                    using (var fileHandle = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var buffer = new byte[4096];
                        var tasks = new List<Task<string>>();
                        bool mustSkip = false;
                        string partialLastLineFromPreviousBuffer = null;
                        while (true)
                        {
                            int bytesRead = fileHandle.Read(buffer, 0, buffer.Length);


                            if (bytesRead == 0)
                            {
                                break;
                            }

                            string word = Encoding.ASCII.GetString(buffer, 0, bytesRead - 1);


                            string[] lines = word.Split('\n');

                            string firstLine = lines[0];

                            if (partialLastLineFromPreviousBuffer != null)
                            {
                                firstLine = firstLine + partialLastLineFromPreviousBuffer;
                            }

                            partialLastLineFromPreviousBuffer = lines[lines.Length - 1];

                            if (partialLastLineFromPreviousBuffer.Length > 1 && partialLastLineFromPreviousBuffer[partialLastLineFromPreviousBuffer.Length - 1] != '\n')
                            {
                                partialLastLineFromPreviousBuffer = lines[lines.Length - 1];
                            }
                            else
                            {
                                partialLastLineFromPreviousBuffer = null;
                            }

                            for (int i = 0; i < lines.Length; i++)
                            {
                                if (mustSkip == false) //in the first read, ignore the first 14 lines
                                {
                                    mustSkip = true;
                                    i = 14;
                                }
                                string currLine = null;
                                if (i == 0)
                                {
                                    currLine = firstLine;
                                }
                                else
                                {
                                    currLine = lines[i];
                                }

                                if (currLine.Contains("*** END OF "))
                                {
                                    break;
                                }


                                string[] words = lines[i].Split(' ');


                                for (int h = 0; h < words.Length; h++)
                                {
                                    word = words[h];


                                    if (word.Length > biggestWord.Length)
                                    {
                                        biggestWord = word;
                                    }
                                }
                            }
                        }
                    }
                    return biggestWord;
                });
             }
    
             public async Task<string> getBiggestWordInDirectoryBaseline(string folderName)
             {
                string BiggestWord = "";
                //
                // Forces to collect all tasks into a List to ensure that all Tasks has started!
                //to do same that is done in java 
                //


                List<Task<string>> list = Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                         .ToList()
                         .Select(file => GetBiggestWordInFileAsync(file))
                         .ToList();



                while (list.Any())
                {
                    Task<string> finishedTask = await Task.WhenAny(list);
                    list.Remove(finishedTask);
                    string biggestWordInFile = await finishedTask;
                    if (biggestWordInFile.Length > BiggestWord.Length)
                    {
                        BiggestWord = biggestWordInFile;
                    }
                }

                //
                // Returns a new task that will complete when all of the Task objects
                // in allTasks collection have completed!
                return BiggestWord;
            }
         }
     }
}
