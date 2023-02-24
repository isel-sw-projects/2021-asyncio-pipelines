using lookwords.BenchmarkFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.testRunnables
{
    class FindBiggestWordRunnable : Runnable
    {
        public override void Run()
        {
            Console.WriteLine("Tests to BenchmarckIOFindBiggestWordsStrategies initiated: ");
            var test = new BenchmarkFindBiggestwordTest();
            test.RunGetBiggestWordBaselineTest();
            test.RunGetBiggestWordSyncTest();
            test.RunGetBiggestWordAsyncEnumerableTest();
            test.RunGetBiggestWordRxTest();
            test.RunGetBiggestWordBaselineTest();
        }
    }
}
