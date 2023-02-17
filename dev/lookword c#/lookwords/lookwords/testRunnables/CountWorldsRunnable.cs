using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.testRunnables
{
    class CountWorldsRunnable : Runnable
    {
        public override void Run()
        {
            Console.WriteLine("Tests to BenchmarckIOCountWordsStrategies initiated: ");
            var test = new BenchmarckIOCountWordsStrategies(folderPath);
            test.RunCountWordsBaseline();
            test.RunSyncTest();
            test.RunAsyncEnumerableTest();
            test.RunRxTest();
        }
    }
}
