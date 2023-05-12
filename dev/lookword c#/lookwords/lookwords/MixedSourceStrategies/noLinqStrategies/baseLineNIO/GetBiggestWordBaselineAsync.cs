using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace lookwords.noLinqStategies.SyncEnum
{
    //file read and file processing are run e different contexts
    namespace lookwords.noLinqStategies.SyncEnum
    {
        public class GetBiggestWordBaselineAsync
        {
            private Task<string> GetBiggestWordInFileAsync(string filename)
            {
                Channel<string> lineChannel = AsyncFiles.readFileAllLinesAsync(filename);

                // Task to process lines
                var processLinesTask = Task.Run(async () =>
                {
                    string biggestWord = "";
                    bool mustSkip = false;
                    int lineCount = 0;

                    while (await lineChannel.Reader.WaitToReadAsync() && lineChannel.Reader.TryRead(out var line))
                    {
                        lineCount++;

                        if (!mustSkip && lineCount <= 14)
                        {
                            continue;
                        }

                        if (line.Contains("*** END OF "))
                        {
                            break;
                        }

                        string[] words = line.Split(' ');

                        for (int h = 0; h < words.Length; h++)
                        {
                            string word = words[h];

                            if (word.Length > biggestWord.Length)
                            {
                                biggestWord = word;
                            }
                        }
                    }

                    return biggestWord;
                });


                return processLinesTask;
            }


            public async Task<string> getBiggestWordInDirectoryAsyncBaseline(string folderName)
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
            


                   await Task.WhenAll(list);

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
                    return BiggestWord;
            }
        }
    }

}
