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
   // public class GetBiggestWordBaseline
   // {
   //     private Task<string> getBiggestWordInFile(string filename)
   //     {
   //         return Task.Factory.StartNew(() =>
   //         {
   //
   //             int count = 0;
   //             string biggestWord = "";
   //             List<Task<string>> lineTasksStack = new List<Task<string>>();
   //
   //
   //             using (StreamReader reader = new StreamReader(filename))
   //             {
   //                 while (!reader.EndOfStream)
   //                 {
   //                    
   //                     Task<string> curr = reader.ReadLineAsync().ContinueWith(tsk =>
   //                     {
   //                         string line;
   //
   //                         line = tsk.Result;
   //
   //                         if (count < 14 || line.Length == 0)
   //                         {
   //                             count++;
   //                             return null;
   //                         }
   //
   //                         string[] wordsInLine = Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' ');
   //
   //                         foreach (string word in wordsInLine)
   //                         {
   //                             if (word.Length >= biggestWord.Length)
   //                             {
   //                                 biggestWord = word;
   //                             }
   //                         }
   //                         return biggestWord;
   //
   //                     });
   //
   //                         lineTasksStack.Add(curr);
   //                 }
   //                 
   //             }
   //
   //             string biggest = "";
   //             bool shouldIgnore = false;
   //             for (int i = 0; i < lineTasksStack.Count && !shouldIgnore; i++)
   //             {
   //
   //                 string curr = lineTasksStack.ElementAt(i).Result;
   //
   //                 if(curr.Contains("*** END OF "))
   //                 {
   //                     shouldIgnore = true;
   //                 }
   //
   //                 if(curr == null)
   //                 {
   //                     continue;
   //                 }
   //
   //                 if (curr.Length > biggest.Length)
   //                 {
   //                     biggest = curr;
   //                 }
   //             }
   //             return biggest;
   //         });
   //        
   // }
   //
   //
   //     public string getBiggestWordInDirectoryAsyncBaseline(string folderName)
   //     {
   //         string BiggestWord = "";
   //         //
   //         // Forces to collect all tasks into a List to ensure that all Tasks has started!
   //         //to do same that is done in java 
   //         //
   //
   //
   //         List<Task<string>> list = Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
   //                  .ToList()
   //                  .Select(file =>getBiggestWordInFile(file))
   //                  .ToList();
   //
   //         for(int i = 0; i < list.Count; i++)
   //         {
   //             if(list[i].Result.Length > BiggestWord.Length)
   //             {
   //                 BiggestWord = list[i].Result;
   //             }
   //         }
   //
   //         //
   //         // Returns a new task that will complete when all of the Task objects
   //         // in allTasks collection have completed!
   //         // 
   //         Console.WriteLine("Biggest word is: {0}", BiggestWord);
   //         return BiggestWord;
   //     }
   // }


    namespace lookwords.noLinqStategies.SyncEnum
    {
        public class GetBiggestWordBaseline
        {
            private async Task<string> GetBiggestWordInFileAsync(string filename)
            {
                int count = 0;
                string biggestWord = "";
                string partialWord = "";

                using (var fileHandle = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] buffer = new byte[4096];
                    var tasks = new List<Task<string>>();
                    var lastByteInPrevBuffer = '\0';

                    while (true)
                    {
                        var bytesRead = await fileHandle.ReadAsync(buffer, 0, buffer.Length);

                        if (bytesRead == 0) break;

                        if (lastByteInPrevBuffer != '\0')
                        {
                            var prevBufferWord = partialWord + Encoding.ASCII.GetString(buffer, 0, 1);
                            if (prevBufferWord.Length > biggestWord.Length && Regex.IsMatch(prevBufferWord, "^[a-zA-Z0-9-]+$"))
                            {
                                biggestWord = prevBufferWord;
                            }
                        }

                        var task = Task.Run(() =>
                        {
                            var startIndex = 0;
                            var endIndex = 0;

                            for (var i = 0; i < bytesRead; i++)
                            {
                                var isWordCharacter = char.IsLetterOrDigit((char)buffer[i]) || buffer[i] == '-';
                                if (isWordCharacter && endIndex == 0)
                                {
                                    startIndex = i;
                                    endIndex = i;
                                }
                                else if (isWordCharacter)
                                {
                                    endIndex = i;
                                }
                                else if (endIndex > startIndex)
                                {
                                    string word = Encoding.ASCII.GetString(buffer, startIndex, endIndex - startIndex + 1);

                                    if (word.Length > biggestWord.Length && Regex.IsMatch(word, "^[a-zA-Z0-9-]+$"))
                                    {
                                        biggestWord = word;
                                    }
                                }

                                if (buffer[i] == '\n')
                                {
                                    count++;
                                }

                                if (count < 14)
                                {
                                    startIndex = endIndex = 0;
                                }
                            }

                            if (endIndex > startIndex)
                            {
                                partialWord = Encoding.ASCII.GetString(buffer, startIndex, endIndex - startIndex + 1);
                            }
                            else
                            {
                                partialWord = "";
                            }

                            lastByteInPrevBuffer =(char) buffer[bytesRead - 1];

                            return biggestWord;
                        });

                        tasks.Add(task);
                    }

                    await Task.WhenAll(tasks);
                }

                if (partialWord.Length > 0 && partialWord.Length > biggestWord.Length && Regex.IsMatch(partialWord, "^[a-zA-Z0-9-]+$"))
                {
                    biggestWord = partialWord;
                }

                return biggestWord;
            }

            public async Task<string> getBiggestWordInDirectoryAsyncBaseline(string folderName)
            {
                var tasks = Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                    .Select(file => GetBiggestWordInFileAsync(file));

                string[] biggestWord = await Task.WhenAll(tasks);

                return biggestWord.OrderByDescending(word => word.Length).First();
            }
        }
    }
}
