using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    internal class GroupWords
    {
        
        public async IAsyncEnumerable<String> Words(string folder,int minLength, int maxLength) {
            IAsyncEnumerable<string> enume = fileUtils.FileUtils.Lines(folder);
            
            await foreach(String line in enume)
            {
                if(line.Contains("*** END OF ") || String.IsNullOrEmpty(line))
                {
                    continue;
                } else
                {
                    String[] words = line.Split(' ');

                    foreach(var word in words.AsEnumerable())
                    {
                        if(word.Length > minLength && word.Length < maxLength)
                        {
                           yield return word;
                        }
                    }
                }
            }
        }
    }
}
