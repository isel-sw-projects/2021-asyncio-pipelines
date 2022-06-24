using lookwords.FileReadStrategies;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.CountCharactersStrategies.SyncEnum
{
    public class EnumerableIOCountCharacterStrategy 
    {
      

        public int countCharactersFromFileSync(string folderName, char character)
        {
            int count = 0;
            return Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                      .ToList()
                      .Select(file => countCharacterOcurrencesInFileSync(file, character))
                      .Aggregate(count, (total, character) => total + character);
                    
        }
    

      
        private int countCharacterOcurrencesInFileSync(string filename, char character)
        {
            return FileUtils.getCharacterSync(filename)
                  .Where(charr => character == charr)                     // Skip empty lines
                  .Count();
        }


    }
    
}
