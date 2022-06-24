
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using lookwords.FileReadStrategies.SyncEnum;
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
            Console.WriteLine("Tests initiated: ");
            //IOFileReadStrategies.folderWordOccurrencesInSizeRangeSync(folderPath, 2, 12);


            //string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");


            BenchmarkRunner.Run<BenchmarckIOCountWordsStrategies>();

           // var test = new BenchmarckIOCountWordsStrategies(folderPath);
           // test.RunSyncTest();
           // test.RunAsyncEnumerableTest();
           // test.RunRxTest();



        }
    }
}
