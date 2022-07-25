
using lookwords;
using lookwords.MixedSourceStrategies.AsyncEnum;
using lookwords.MixedSourceStrategies.BiggestWordFileReadStrategies.SyncEnum;
using lookwords.MixedSourceStrategies.FileReadStrategies.AsyncEnum;
using lookwords.MixedSourceStrategies.FileReadStrategies.RxNet;
using lookwords.MixedSourceStrategies.FileReadStrategies.SyncEnum;
using lookwords.MixedSourceStrategies.RxNet;
using lookwords.noLinqStategies.SyncEnum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class UnitTest_countwords
    {

        [TestMethod]
        public void CountWordsSync()
        {
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            string folderPath = @Environment.GetEnvironmentVariable("TESE_TESTE_PATH");
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            EnumerableIOFileReadStrategy test = new EnumerableIOFileReadStrategy();
            ConcurrentDictionary<string, int> ret = test.countWordsFromFileSync(folderPath, 1, 8);

            Assert.IsTrue(ret.Count == 15);

            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test"], 10 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.AreEqual(ret["1"], 1 * 4);
            Assert.AreEqual(ret["2"], 2 * 4);
            Assert.AreEqual(ret["3"], 3 * 4);
            Assert.AreEqual(ret["4"], 4 * 4);
            Assert.AreEqual(ret["5"], 5 * 4);
            Assert.AreEqual(ret["6"], 6 * 4);
            Assert.AreEqual(ret["7"], 7 * 4);
            Assert.AreEqual(ret["8"], 8 * 4);
            Assert.AreEqual(ret["9"], 9 * 4);
            Assert.AreEqual(ret["10"], 10 * 4);
            Assert.AreEqual(ret["15"], 1 * 4);

            ret = test.countWordsFromFileSync(folderPath, 2, 8);


            Assert.IsTrue(ret.Count == 6);
            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test"], 10 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.AreEqual(ret["10"], 10 * 4);
            Assert.AreEqual(ret["15"], 1 * 4);

            ret = test.countWordsFromFileSync(folderPath, 5, 8);

            Assert.IsTrue(ret.Count == 3);

            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.IsFalse(ret.ContainsKey("test"));
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.IsFalse(ret.ContainsKey("10"));
            Assert.IsFalse(ret.ContainsKey("11"));
        }

        [TestMethod]
        public void CountWordsASyncEnum()
        {
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            string folderPath = @Environment.GetEnvironmentVariable("TESE_TESTE_PATH");
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            AsyncEnumerableIOFileReadStrategy test = new AsyncEnumerableIOFileReadStrategy();

            Task<ConcurrentDictionary<string, int>> task = test.countWordsFromFileAsync(folderPath, 1, 8);
            task.Wait();

            ConcurrentDictionary<string, int> ret = task.Result;

            Assert.IsTrue(ret.Count == 15);

            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test"], 10 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.AreEqual(ret["1"], 1 * 4);
            Assert.AreEqual(ret["2"], 2 * 4);
            Assert.AreEqual(ret["3"], 3 * 4);
            Assert.AreEqual(ret["4"], 4 * 4);
            Assert.AreEqual(ret["5"], 5 * 4);
            Assert.AreEqual(ret["6"], 6 * 4);
            Assert.AreEqual(ret["7"], 7 * 4);
            Assert.AreEqual(ret["8"], 8 * 4);
            Assert.AreEqual(ret["9"], 9 * 4);
            Assert.AreEqual(ret["10"], 10 * 4);
            Assert.AreEqual(ret["15"], 1 * 4);

            task = test.countWordsFromFileAsync(folderPath, 2, 8);
            task.Wait();

            ret = task.Result;


            Assert.IsTrue(ret.Count == 6);
            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test"], 10 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.AreEqual(ret["10"], 10 * 4);
            Assert.AreEqual(ret["15"], 1 * 4);

            task = test.countWordsFromFileAsync(folderPath, 5, 8);
            task.Wait();

            ret = task.Result;

            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.IsFalse(ret.ContainsKey("test"));
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.IsFalse(ret.ContainsKey("10"));
            Assert.IsFalse(ret.ContainsKey("11"));
        }

        [TestMethod]
        public void CountWordsRXTest()
        {
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            string folderPath = @Environment.GetEnvironmentVariable("TESE_TESTE_PATH");
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            RXNetIOFileReadStrategy test = new RXNetIOFileReadStrategy();

            Task<ConcurrentDictionary<string, int>> task = test.countWordsFromFileAsync(folderPath, 1, 8);
            task.Wait();

            ConcurrentDictionary<string, int> ret = task.Result;

            Assert.IsTrue(ret.Count == 15);

            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test"], 10 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.AreEqual(ret["1"], 1 * 4);
            Assert.AreEqual(ret["2"], 2 * 4);
            Assert.AreEqual(ret["3"], 3 * 4);
            Assert.AreEqual(ret["4"], 4 * 4);
            Assert.AreEqual(ret["5"], 5 * 4);
            Assert.AreEqual(ret["6"], 6 * 4);
            Assert.AreEqual(ret["7"], 7 * 4);
            Assert.AreEqual(ret["8"], 8 * 4);
            Assert.AreEqual(ret["9"], 9 * 4);
            Assert.AreEqual(ret["10"], 10 * 4);
            Assert.AreEqual(ret["15"], 1 * 4);

            task = test.countWordsFromFileAsync(folderPath, 2, 8);
            task.Wait();

            ret = task.Result;


            Assert.IsTrue(ret.Count == 6);
            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test"], 10 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.AreEqual(ret["10"], 10 * 4);
            Assert.AreEqual(ret["15"], 1 * 4);

            task = test.countWordsFromFileAsync(folderPath, 5, 8);
            task.Wait();

            ret = task.Result;

            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.IsFalse(ret.ContainsKey("test"));
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.IsFalse(ret.ContainsKey("10"));
            Assert.IsFalse(ret.ContainsKey("11"));
        }


        [TestMethod]
        public void CountWordsSync_NoLinq()
        {
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            string folderPath = @Environment.GetEnvironmentVariable("TESE_TESTE_PATH");
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            CountWordsWithoutLinqStrategy test = new CountWordsWithoutLinqStrategy();
            ConcurrentDictionary<string, int> ret = test.countWordsFromFileSync(folderPath, 1, 8);

            Assert.IsTrue(ret.Count == 15);

            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test"], 10 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.AreEqual(ret["1"], 1 * 4);
            Assert.AreEqual(ret["2"], 2 * 4);
            Assert.AreEqual(ret["3"], 3 * 4);
            Assert.AreEqual(ret["4"], 4 * 4);
            Assert.AreEqual(ret["5"], 5 * 4);
            Assert.AreEqual(ret["6"], 6 * 4);
            Assert.AreEqual(ret["7"], 7 * 4);
            Assert.AreEqual(ret["8"], 8 * 4);
            Assert.AreEqual(ret["9"], 9 * 4);
            Assert.AreEqual(ret["10"], 10 * 4);
            Assert.AreEqual(ret["15"], 1 * 4);

            ret = test.countWordsFromFileSync(folderPath, 2, 8);


            Assert.IsTrue(ret.Count == 6);
            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test"], 10 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.AreEqual(ret["10"], 10 * 4);
            Assert.AreEqual(ret["15"], 1 * 4);

            ret = test.countWordsFromFileSync(folderPath, 5, 8);

            Assert.IsTrue(ret.Count == 3);

            Assert.AreEqual(ret["test1"], 25 * 4);
            Assert.AreEqual(ret["test2"], 20 * 4);
            Assert.IsFalse(ret.ContainsKey("test"));
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.IsFalse(ret.ContainsKey("10"));
            Assert.IsFalse(ret.ContainsKey("11"));
        }

        [TestMethod]
        public void BiggestWordSync_Sync()
        {
            string folderPath = @Environment.GetEnvironmentVariable("TESE_TESTE_PATH");
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            EnumerableIOFindBiggestWordStrategy test = new EnumerableIOFindBiggestWordStrategy();
            string word = test.getBiggestWordInDirectory(folderPath);

            Assert.AreEqual(word, "bigggest33333333333333333333333333333333");
        }

        [TestMethod]
        public void BiggestWordSync_NoLinq()
        {
            string folderPath = @Environment.GetEnvironmentVariable("TESE_TESTE_PATH");
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            BiggestWordNoLinq test = new BiggestWordNoLinq();
            string word = test.getBiggestWordInDirectory(folderPath);

            Assert.AreEqual(word, "bigggest33333333333333333333333333333333");
        }

        [TestMethod]
        public void BiggestWordSync_ASync()
        {
            string folderPath = @Environment.GetEnvironmentVariable("TESE_TESTE_PATH");
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            AsyncEnumerableIOBiggestWordStrategy test = new AsyncEnumerableIOBiggestWordStrategy();
            Task<string> task = test.getBiggestWordInDirectory(folderPath);
            task.Wait();

            string word = task.Result;

            Assert.AreEqual(word, "bigggest33333333333333333333333333333333");
        }

        [TestMethod]
        public async Task BiggestWordSync_Rx()
        {
            

            string folderPath = @Environment.GetEnvironmentVariable("TESE_TESTE_PATH");
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            RXNetIOBiggestWordStrategy test = new RXNetIOBiggestWordStrategy();

            IObservable<string> obsrvable = new RXNetIOBiggestWordStrategy().getBiggestWordInDirectory(folderPath);

            var t = new TaskCompletionSource<object>();
            obsrvable.Subscribe(str => t.TrySetResult(str));


            await t.Task;

            Assert.AreEqual(t.Task.Result, "bigggest33333333333333333333333333333333");
        }
    }
}