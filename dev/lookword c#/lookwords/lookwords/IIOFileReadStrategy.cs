using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    public interface IIOFileReadStrategy
    {
        Task<ConcurrentDictionary<string, int>> countWordsFromFileAsync(string folderName, int minWordLength, int maxWordLength);

        ConcurrentDictionary<string, int> countWordsFromFileSync(string folderName, int minWordLength, int maxWordLength);

        Task<int> countCharactersFromFileAsync(string folderName, char character);

        int countCharactersFromFileSync(string folderName, char character);
    }
}
