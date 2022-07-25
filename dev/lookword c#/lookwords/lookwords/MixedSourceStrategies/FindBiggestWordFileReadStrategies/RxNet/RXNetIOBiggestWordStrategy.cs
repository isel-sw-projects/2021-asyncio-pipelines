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

namespace lookwords.MixedSourceStrategies.RxNet
{
    public class RXNetIOBiggestWordStrategy
    {

        /// <summary>
        /// Collects all distinct words of given filePath into words Dictionary, which is shared across concurrent threads.
        /// ??? Not sure about the concurrent behavior of ForEachAsync ???
        /// </summary>
        private IObservable<string> findBiggestWordInFile(string filePath)
        {

            return new ReadFolderFilesObservable(filePath)
                .Where(line => line.Length != 0)                           // Skip empty lines
                .Skip(14)                                                  // Skip gutenberg header
                .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                .Select(line => Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled)
                                        .Split(' ')
                                        .Max(arr => arr))
                .Aggregate("", (biggest, curr) => curr.Length > biggest.Length ? curr : biggest)
                .SingleAsync();
        }

        //RXNET implementation
        public IObservable<string> getBiggestWordInDirectory(string folderPath)
        {
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            return Directory
                            .GetFiles(folderPath, "*.txt", SearchOption.AllDirectories)
                            .ToObservable()
                            .SelectMany(file => findBiggestWordInFile(file))
                            .Aggregate("", (biggest, curr) => curr.Length > biggest.Length ? curr : biggest)
                            .LastOrDefaultAsync();
        }
    }
}
