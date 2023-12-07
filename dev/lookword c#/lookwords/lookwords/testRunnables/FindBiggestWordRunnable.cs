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
            test.RunGetBiggestWordBaselineTest();
            test.RunGetBiggestWordBaselineTest();
            test.RunBiggestWordBaselineTaskMultithreadBlockingRead();
            test.RunBiggestWordBaselineTaskMultithreadBlockingRead();
            test.RunBiggestWordBaselineTaskMultithreadBlockingRead();//each task per file, block read
            test.RunGetBiggestWordAsyncBaseline();
            test.RunGetBiggestWordAsyncBaseline();
            test.RunGetBiggestWordAsyncBaseline(); // asyncc read use awai async
                                                   // test.RunGetBiggestWordAsyncBaselineTest();
           
            // test.RunGetBiggestWordBaselineTestNoContinueWith();
            test.RunGetBiggestWordLinqSyncTest();
            test.RunGetBiggestWordLinqSyncTest();
            test.RunGetBiggestWordLinqSyncTest();
            test.RunGetBiggestWordAsyncEnumerableTest();
            test.RunGetBiggestWordAsyncEnumerableTest();
            test.RunGetBiggestWordAsyncEnumerableTest();
            //test.RunGetBiggestWordRxTest();
            test.RunGetBiggestWordRxAsyncFileReadTest();
            test.RunGetBiggestWordRxAsyncFileReadTest();
            test.RunGetBiggestWordRxAsyncFileReadTest();


        }
    }
}
