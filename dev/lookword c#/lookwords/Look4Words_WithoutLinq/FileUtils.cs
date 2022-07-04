
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Look4Words_WithoutLinq
{
    public class FileUtils
    {

        public static IEnumerable<String> getLinesSync(String file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                   yield return reader.ReadLine();
                }
            }

        }

        public static IEnumerable<char> getCharacterSync(String file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string words = reader.ReadLine();

                    foreach (char character in words)
                    {
                        yield return character;
                    }
                }
            }

        }

        public static async IAsyncEnumerable<String> getLinesAsyncEnum(String file)
        {

            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    yield return await reader.ReadLineAsync();
                }
            }
            
        }

        public static async IAsyncEnumerable<char> getCharacterAsyncEnum(String file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string words = await reader.ReadLineAsync();
                    //Console.WriteLine(words);
                    foreach (char character in words)
                    {
                        yield return character;
                    }
                }
            }

        }
       
    }
}
