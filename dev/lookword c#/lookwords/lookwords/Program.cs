
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using lookwords.BenchmarkFiles;
using lookwords.BiggestWordFileReadStrategies.SyncEnum;
using lookwords.RxNet;
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

            string input = "";
            while (!input.Equals("esc")) {
                Console.WriteLine(" 1 - To find biggest words in test directory,\n 2 - To count all word occurrences in test directory \n a - run all \n esc - To leave");

                input = Console.ReadLine();
                if (input.Equals("1"))
                {
                    Console.WriteLine("Tests to BenchmarckIOCountWordsStrategies initiated: ");
                    var test = new BenchmarckIOCountWordsStrategies(folderPath);
                    test.RunSyncTestWOLinq();
                    test.RunSyncTest();
                    test.RunAsyncEnumerableTest();
                    test.RunRxTest();
                } else if (input.Equals("2"))
                {
                    Console.WriteLine("Tests to BenchmarckIOCountWordsStrategies initiated: ");
                    var test = new BenchmarkFindBiggestwordTest();
                    test.RunGetBiggestWordWOLinqSyncTest();
                    test.RunGetBiggestWordSyncTest();
                    test.RunGetBiggestWordAsyncEnumerableTest();
                    test.RunGetBiggestWordRxTest();
                    

                }
                else if(input.Equals("a")) {
                    Console.WriteLine("Tests to BenchmarckIOCountWordsStrategies initiated: ");
                    var test2 = new BenchmarckIOCountWordsStrategies(folderPath);
                    test2.RunSyncTestWOLinq();
                    test2.RunSyncTest();
                    test2.RunAsyncEnumerableTest();
                    test2.RunRxTest();

                    Console.WriteLine("\nTests to BenchmarkFindBiggestwordStrategies initiated: ");
                    var test = new BenchmarkFindBiggestwordTest();
                    test.RunGetBiggestWordWOLinqSyncTest();
                    test.RunGetBiggestWordSyncTest();
                    test.RunGetBiggestWordAsyncEnumerableTest();
                    test.RunGetBiggestWordRxTest();

                }

            }

        }
    }
}
