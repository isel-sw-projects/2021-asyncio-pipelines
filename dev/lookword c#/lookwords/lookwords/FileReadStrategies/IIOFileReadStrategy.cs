using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.FileReadStrategies
{
    public interface IIOFileReadStrategy
    {
        Task<ConcurrentDictionary<string, int>> getDistingWordsFromFileAsyncEnumerable(string folderName, int minWordLength, int maxWordLength);

        ConcurrentDictionary<string, int> getDistingWordsFromFileSync(string folderName, int minWordLength, int maxWordLength);
    }
}
