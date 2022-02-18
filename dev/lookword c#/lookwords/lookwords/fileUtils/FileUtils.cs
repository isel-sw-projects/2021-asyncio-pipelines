using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.fileUtils
{
    internal class FileUtils
    {

        public static async IAsyncEnumerable<String> Lines(String folderPath)
        {
            string[] allfiles = Directory.GetFiles(folderPath, ".txt", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    yield return await reader.ReadLineAsync();
                }
            }
        }
     
    }
}
