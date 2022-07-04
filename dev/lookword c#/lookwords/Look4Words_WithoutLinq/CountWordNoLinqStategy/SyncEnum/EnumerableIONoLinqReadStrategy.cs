
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Look4Words_WithoutLinq
{
    public static class EnumerableExtensionsWithOutLinq
    {
        public static void ForEach<T>(this IEnumerable<T> src, Action<T> cons)
        {
            foreach (var item in src)
            {
                cons(item);
            }
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> src, Func<T, bool> predicate)
        {
            foreach (var item in src)
            {
                if (predicate.Invoke(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> Sip<T>(this IEnumerable<T> source, int count)
        {

            int toZero = count;
            IEnumerator<T> enumerator = source.GetEnumerator();

            if (source is IList<T>)
            {
                IList<T> list = (IList<T>)source;
                for (int i = count; i < list.Count; i++)
                {
                    enumerator.MoveNext();
                    yield return list[i];
                }
            }
            else if (source is IList)
            {
                IList list = (IList)source;
                for (int i = count; i < list.Count; i++)
                {
                    enumerator.MoveNext();
                    yield return (T)list[i];
                }
            }
            else
            {
                while (toZero-- > 0)
                {
                    enumerator.MoveNext();
                }

                if (toZero <= 0)
                {
                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

       public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> source, Func<T, bool> pred)
       {
           foreach (T item in source)
           {
               if (!pred(item))
               {
                   break;
               }
               yield return item;
           }
       }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            foreach (TSource item in source)
            {
                foreach (TResult item2 in selector(item))
                {
                    yield return item2;
                }
            }
        }
    }

    public class EnumerableIOFileReadWithoutLinqStrategy 
    {

        private void parseFileDistinctWordsIntoDictionary(string filename, int minWordSize, int maxWordSize, ConcurrentDictionary<string, int> words)
        {
            //Console.WriteLine(filename);
            FileUtils.getLinesSync(filename)
                 .Where(line => line.Length != 0)                           // Skip empty lines
                 .Skip(14)                                                  // Skip gutenberg header
                 .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                 .SelectMany(line => Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' '))
                 .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize)
                 .ForEach((word) => {
                     //Console.WriteLine(word);
                     words.AddOrUpdate(word, 1, (k, v) => v + 1);
                 }); // Merge words in dictionary.
        }


        public ConcurrentDictionary<string, int> countWordsFromFileSync(string folderName, int minWordLength, int maxWordLength)
        {
            var words = new ConcurrentDictionary<string, int>();
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                     .ToList()
                     .ForEach(file => parseFileDistinctWordsIntoDictionary(file, minWordLength, maxWordLength, words));
            //
            // Returns a new task that will complete when all of the Task objects
            // in allTasks collection have completed!
            // 
            return words;
        }

    }
}
