using lookwords.FileReadStrategies;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lookwords.BiggestWordFileReadStrategies.SyncEnum
{
    public class EnumerableIOFindBiggestWordStrategy 
    {

       private string parseFileDistinctWordsIntoDictionary(string filename)
       {
         
         return FileUtils.getLinesSync(filename)
                .Where(line => line.Length != 0)                           // Skip empty lines
                .Skip(14)                                                  // Skip gutenberg header
                .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
                .Select(line => Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' ').Max()) //?? to review ?? 
                .Aggregate( string.Empty, (biggest, current) => current.Length > biggest.Length ? current : biggest);
                 
        }


        public string getBiggestWordInDirectory(string folderName)
        {
            //
            // Forces to collect all tasks into a List to ensure that all Tasks has started!
            //
            return Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                     .ToList()
                     .Select(file => parseFileDistinctWordsIntoDictionary(file))
                     .Aggregate("", (biggest, current) => current.Length > biggest.Length ? current : biggest); 
        }

    }
    
}
