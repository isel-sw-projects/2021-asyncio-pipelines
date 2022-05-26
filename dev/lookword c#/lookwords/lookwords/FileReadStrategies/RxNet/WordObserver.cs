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
        Dictionary<String, int> words_dict;

        public WordObserver(Dictionary<String, int> words_dict, int minLength, int maxLength)
        {
            this.words_dict = words_dict;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public int MinLength { get; }
        public int MaxLength { get; }

        public void OnCompleted()
        {
            foreach (var word in words_dict)
            {
                Console.WriteLine("Value: {0}, Count: {1}", word.Key, word.Value);
            }

            Console.WriteLine("Iteration completed");
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }


        public void OnNext(string word)
        {
            Console.WriteLine(word);
            
        }
    }
}
