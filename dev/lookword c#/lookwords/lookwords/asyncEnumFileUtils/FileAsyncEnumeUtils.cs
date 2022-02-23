using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.fileUtils
{
    internal class FileAsyncEnumeUtils
    {

        public static async IAsyncEnumerable<String> GetLinesFromFolderTree(String folderPath)
        {
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {
                using (StreamReader reader = new StreamReader(file))

                    while (!reader.EndOfStream)
                    {
                        yield return await reader.ReadLineAsync();
                    }
                }
            }
        }
     
}
