using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.RxNet
{
    public class WordObserver : IObserver<string>
    {
        ConcurrentDictionary<String, int> words_dict;
        private Boolean isFinished;

        public WordObserver(ConcurrentDictionary<String, int> words_dict, Boolean isFinished)
        {
            this.words_dict = words_dict;
            this.isFinished = isFinished;
        }

        public void OnCompleted()
        {
            Console.WriteLine("Iteration completed");
            isFinished = true;
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(string word)
        {
            Console.WriteLine(word);
            if (words_dict.ContainsKey(word))
            {
                int nextValue;
                while (!words_dict.TryRemove(word, out nextValue)) { };

                while (!words_dict.TryAdd(word, nextValue + 1)) { };
            }
            else
            {
                while (!words_dict.TryAdd(word, 1)) { };
            }

        }
    }
}
