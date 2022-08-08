using lookwords.MixedSourceStrategies.RxNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lookwords.MixedSourceStrategies.FindWordStrategies.RxNet
{
    public class RXNetIOFindWordStrategy
    {

        /// <summary>
        /// Collects all distinct words of given filePath into words Dictionary, which is shared across concurrent threads.
        /// ??? Not sure about the concurrent behavior of ForEachAsync ???
        /// </summary>
        private IObservable<bool> findWordInFile(string filePath, string word)
        {
            return new ReadFolderFilesObservable(filePath)
                .Where(line => line.Length != 0)                           // Skip empty lines
                .Skip(14)                                                  // Skip gutenberg header
                .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                .Any(line =>
                {
                    if (line.Contains(word))
                    {
                        Console.WriteLine("found in: " + filePath);
                        return true;
                    }
                    return false;

                });
        }

        //RXNET implementation
        public IObservable<bool> findWordInDirectory(string folderPath, string word)
        {
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            return Directory
                            .GetFiles(folderPath, "*.txt", SearchOption.AllDirectories)
                            .ToObservable()
                            .SelectMany(file => findWordInFile(file, word))
                            .Any(obs => obs);
        }
    }
}
