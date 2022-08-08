using lookwords.MixedSourceStrategies;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lookwords.MixedSourceStrategies.FindWordStrategies.SyncEnum
{
    public class EnumerableIOFindWordStrategy 
    {

       private bool findWordInFile(string filename, string word)
       {
            return FileUtils.getLinesSync(filename)
                   .Where(line => line.Length != 0)                           // Skip empty lines
                   .Skip(14)                                                  // Skip gutenberg header
                   .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                   .Any(line =>
                   {
                       if (line.Contains(word))
                       {
                           Console.WriteLine("found in: " + filename);
                           return true;
                       }
                       return false;
                   });


        }


        public bool findWordInDirectory(string folderName, string word)
        {
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            return Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                     .ToList()
                     .Select(file => findWordInFile(file, word))
                     .Any(obs => obs);
        }

    }
    
}
