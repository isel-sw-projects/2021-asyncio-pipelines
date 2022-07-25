using lookwords.BenchmarkFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.testRunnables
{
    class TestStrategiesWithSyncSourcesWithParallelismRunnable : Runnable
    {
        public override void Run() { 
        
            Console.WriteLine("Starting Test Strategies With Sync Sources With Parallelism ");
            var test = new BenchmarkIOStrategiesWSyncAndAsParallel();
            test.RunAsyncEnumerableTest();
            //test.RunRxTest();
            test.RunGetBiggestWordAsyncEnumerableTest();
            //test.RunGetBiggestWordRxTest();
            
        }
    }
}
