using Look4Words_WithoutLinq;
using lookwords;
using lookwords.FileReadStrategies.AsyncEnum;
using lookwords.FileReadStrategies.SyncEnum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void CountWordsSync()
        {
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            string folderPath = @Environment.GetEnvironmentVariable("TESE_TESTE_PATH");
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            EnumerableIOFileReadStrategy test = new EnumerableIOFileReadStrategy();
            ConcurrentDictionary<string, int> ret = test.countWordsFromFileSync(folderPath, 1, 8);

            Assert.IsTrue(ret.Count == 14);

            Assert.AreEqual(ret["test1"], 25 * 2);
            Assert.AreEqual(ret["test"], 10 * 2);
            Assert.AreEqual(ret["test2"], 20 * 2);
            Assert.AreEqual(ret["1"], 1 * 2);
            Assert.AreEqual(ret["2"], 2 * 2);
            Assert.AreEqual(ret["3"], 3 * 2);
            Assert.AreEqual(ret["4"], 4 * 2);
            Assert.AreEqual(ret["5"], 5 * 2);
            Assert.AreEqual(ret["6"], 6 * 2);
            Assert.AreEqual(ret["7"], 7 * 2);
            Assert.AreEqual(ret["8"], 8 * 2);
            Assert.AreEqual(ret["9"], 9 * 2);
            Assert.AreEqual(ret["10"], 10 * 2);
            Assert.AreEqual(ret["15"], 1 * 2);

            ret = test.countWordsFromFileSync(folderPath, 2, 8);


            Assert.IsTrue(ret.Count == 5);
            Assert.AreEqual(ret["test1"], 25 * 2);
            Assert.AreEqual(ret["test"], 10 * 2);
            Assert.AreEqual(ret["test2"], 20 * 2);
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.AreEqual(ret["10"], 10 * 2);
            Assert.AreEqual(ret["15"], 1 * 2);

            ret = test.countWordsFromFileSync(folderPath, 5, 8);

            Assert.IsTrue(ret.Count == 2);

            Assert.AreEqual(ret["test1"], 25 * 2);
            Assert.AreEqual(ret["test2"], 20 * 2);
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

            Assert.IsTrue(ret.Count == 14);

            Assert.AreEqual(ret["test1"], 25 * 2);
            Assert.AreEqual(ret["test"], 10 * 2);
            Assert.AreEqual(ret["test2"], 20 * 2);
            Assert.AreEqual(ret["1"], 1 * 2);
            Assert.AreEqual(ret["2"], 2 * 2);
            Assert.AreEqual(ret["3"], 3 * 2);
            Assert.AreEqual(ret["4"], 4 * 2);
            Assert.AreEqual(ret["5"], 5 * 2);
            Assert.AreEqual(ret["6"], 6 * 2);
            Assert.AreEqual(ret["7"], 7 * 2);
            Assert.AreEqual(ret["8"], 8 * 2);
            Assert.AreEqual(ret["9"], 9 * 2);
            Assert.AreEqual(ret["10"], 10 * 2);
            Assert.AreEqual(ret["15"], 1 * 2);

            task = test.countWordsFromFileAsync(folderPath, 2, 8);
            task.Wait();

            ret = task.Result;


            Assert.IsTrue(ret.Count == 5);
            Assert.AreEqual(ret["test1"], 25 * 2);
            Assert.AreEqual(ret["test"], 10 * 2);
            Assert.AreEqual(ret["test2"], 20 * 2);
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.AreEqual(ret["10"], 10 * 2);
            Assert.AreEqual(ret["15"], 1 * 2);

            task = test.countWordsFromFileAsync(folderPath, 5, 8);
            task.Wait();

            ret = task.Result;

            Assert.AreEqual(ret["test1"], 25 * 2);
            Assert.AreEqual(ret["test2"], 20 * 2);
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
            EnumerableIOFileReadWithoutLinqStrategy test = new EnumerableIOFileReadWithoutLinqStrategy();
            ConcurrentDictionary<string, int> ret = test.countWordsFromFileSync(folderPath, 1, 8);

            Assert.IsTrue(ret.Count == 14);

            Assert.AreEqual(ret["test1"], 25 * 2);
            Assert.AreEqual(ret["test"], 10 * 2);
            Assert.AreEqual(ret["test2"], 20 * 2);
            Assert.AreEqual(ret["1"], 1 * 2);
            Assert.AreEqual(ret["2"], 2 * 2);
            Assert.AreEqual(ret["3"], 3 * 2);
            Assert.AreEqual(ret["4"], 4 * 2);
            Assert.AreEqual(ret["5"], 5 * 2);
            Assert.AreEqual(ret["6"], 6 * 2);
            Assert.AreEqual(ret["7"], 7 * 2);
            Assert.AreEqual(ret["8"], 8 * 2);
            Assert.AreEqual(ret["9"], 9 * 2);
            Assert.AreEqual(ret["10"], 10 * 2);
            Assert.AreEqual(ret["15"], 1 * 2);

            ret = test.countWordsFromFileSync(folderPath, 2, 8);


            Assert.IsTrue(ret.Count == 5);
            Assert.AreEqual(ret["test1"], 25 * 2);
            Assert.AreEqual(ret["test"], 10 * 2);
            Assert.AreEqual(ret["test2"], 20 * 2);
            Assert.IsFalse(ret.ContainsKey("1"));
            Assert.IsFalse(ret.ContainsKey("2"));
            Assert.IsFalse(ret.ContainsKey("3"));
            Assert.IsFalse(ret.ContainsKey("4"));
            Assert.IsFalse(ret.ContainsKey("5"));
            Assert.IsFalse(ret.ContainsKey("6"));
            Assert.IsFalse(ret.ContainsKey("7"));
            Assert.IsFalse(ret.ContainsKey("8"));
            Assert.IsFalse(ret.ContainsKey("9"));
            Assert.AreEqual(ret["10"], 10 * 2);
            Assert.AreEqual(ret["15"], 1 * 2);

            ret = test.countWordsFromFileSync(folderPath, 5, 8);

            Assert.IsTrue(ret.Count == 2);

            Assert.AreEqual(ret["test1"], 25 * 2);
            Assert.AreEqual(ret["test2"], 20 * 2);
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
    }
}