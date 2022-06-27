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


      //  private void parseFileDistinctWordsIntoDictionary(string filename, ConcurrentDictionary<string, int> words)
      //  {
      //     int wordLength = 0;
      //
      //      FileUtils.getLinesSync(filename)
      //          .Where(line => line.Length != 0)                           // Skip empty lines
      //          .Skip(14)                                                  // Skip gutenberg header
      //          .TakeWhile(line => !line.Contains("*** END OF "))          // Skip gutenberg footnote
      //          .SelectMany(line => Regex.Replace(line, "[^a-zA-Z0-9 -]+", "", RegexOptions.Compiled).Split(' '))
      //          .Select(line => Regex.Replace(line, "[^a-zA-Z0- 9-]+", "", RegexOptions.Compiled).Split(' ')) //?? to review ?? 
      //          .Select((arr) => arr.OrderByDescending(s => s.Length))
      //          .First();
      //  }
      //
      //
      //  public ConcurrentDictionary<string, int> findBiggestWordSync(string folderName)
      //  {
      //      //
      //      // Forces to collect all tasks into a List to ensure that all Tasks has started!
      //      //
      //      Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
      //               .ToList()
      //               .ForEach(file => parseFileDistinctWordsIntoDictionary(file))
      //               .Select((arr) => arr.OrderByDescending(s => s.Length))
      //          .First();
      //      //
      //      // Returns a new task that will complete when all of the Task objects
      //      // in allTasks collection have completed!
      //      // 
      //      return words;
      //  }
      //

    }
    
}
