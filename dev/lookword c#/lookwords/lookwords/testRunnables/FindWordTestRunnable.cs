using lookwords.BenchmarkFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.testRunnables
{
    internal class FindWordTestRunnable : Runnable
    {
        public override void Run()
        {
            Console.WriteLine("Tests to BenchmarckIOFindWordsStrategies initiated: ");
            var test = new BenchmarkFindWordStrategies();

            string word1 = "ornitorrinco";

            Console.WriteLine("Finding: "+ word1);

            test.RunFindWorddRxTest(word1);
            test.RunFindWordAsyncTest(word1);
            test.RunGetFindWordSyncTest(word1);

            string word2 = "Quantum";

            Console.WriteLine("Finding: " + word2);

            test.RunFindWorddRxTest(word2);
            test.RunFindWordAsyncTest(word2);
            test.RunGetFindWordSyncTest(word2);

            string word3 = "wordthatdonotexist";

            Console.WriteLine("Finding: " + word3);

            test.RunFindWorddRxTest(word3);
            test.RunFindWordAsyncTest(word3);
            test.RunGetFindWordSyncTest(word3);
        }
    }
}
