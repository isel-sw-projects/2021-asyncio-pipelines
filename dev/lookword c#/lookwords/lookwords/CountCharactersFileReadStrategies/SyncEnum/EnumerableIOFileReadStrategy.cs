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
    public class EnumerableIOCountCharacterStrategy : IIOFileReadStrategy
    {
        public Task<int> countCharactersFromFileAsync(string folderName, char character)
        {
            throw new NotImplementedException();
        }

        public int countCharactersFromFileSync(string folderName, char character)
        {
            int count = 0;
            return Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories)
                      .ToList()
                      .Select(file => countCharacterOcurrencesInFileSync(file, character))
                      .Aggregate(count, (total, character) => total + character);
                    
        }
    

        public Task<ConcurrentDictionary<string, int>> countWordsFromFileAsync(string folderName, int minWordLength, int maxWordLength)
        {
            throw new NotImplementedException();
        }

        public ConcurrentDictionary<string, int> countWordsFromFileSync(string folderName, int minWordLength, int maxWordLength)
        {
            throw new NotImplementedException();
        }

        private int countCharacterOcurrencesInFileSync(string filename, char character)
        {
            return FileUtils.getCharacterSync(filename)
                  .Where(charr => character == charr)                     // Skip empty lines
                  .Count();
        }


    }
    
}
