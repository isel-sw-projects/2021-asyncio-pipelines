
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using lookwords.RxNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    [MemoryDiagnoser]
    internal class Program
    {
        static private string folderPath = @"F:\escola\MEIC\TESE\dev\lookwords-master\src\main\resources\gutenberg\";
        static ConcurrentDictionary<String, int> words_dict = new ConcurrentDictionary<String, int>();

        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<MemoryBenchmarkRunner>();
            Console.ReadLine();
        }

    }
}
