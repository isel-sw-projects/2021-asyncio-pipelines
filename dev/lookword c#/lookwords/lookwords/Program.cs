
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using lookwords.BenchmarkFiles;
using lookwords.testRunnables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    class Program
    {

        static void Main(string[] args)
        {
           //var summary = BenchmarkRunner.Run<IOTStategiesBenchmark>();
            
            //IOFileReadStrategies.folderWordOccurrencesInSizeRangeSync(folderPath, 2, 12);
            string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");

            Dictionary<string, Runnable> dictionary = new Dictionary<string, Runnable>();
            dictionary.Add("1", new CountWorldsRunnable());
            dictionary.Add("2", new FindBiggestWordRunnable());
            dictionary.Add("3", new TestStrategiesWithSyncSourcesRunnable());
            dictionary.Add("4", new TestStrategiesWithSyncSourcesWithParallelismRunnable());
            dictionary.Add("5", new FindWordTestRunnable());

            string input = "";
            
            
            
            
            while (!input.Equals("esc")) {
                Console.WriteLine("\n\n 1 - Count words in test directory," +
                    "\n 2 - Find biggest word in test directory " +
                    "\n 3 - Run tests with synchronous sources only " +
                    "\n 4 - Run tests with synchronous sources only but with paralelism " +
                    "\n 5 - Run Find word tests " +
                    "\n esc - To leave \n \n \n");

                input = Console.ReadLine();

                Runnable test = null;
                bool res = dictionary.TryGetValue(input, out test);

                if(res)
                {
                    test.Run();
                }

            }

        }
    }
}
