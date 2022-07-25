using lookwords.BenchmarkFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.testRunnables
{
    class TestStrategiesWithSyncSourcesRunnable : Runnable
    {
        public override void Run()
        {
            Console.WriteLine("Starting Test Strategies With Sync Sources  ");
            var test = new BenchmarkIOStrategiesWithSyncSourceOnly();
            test.RunAsyncEnumerableTest();
            test.RunRxTest();
            test.RunGetBiggestWordAsyncEnumerableTest();
            test.RunGetBiggestWordRxTest();
            
        }
    }
}
