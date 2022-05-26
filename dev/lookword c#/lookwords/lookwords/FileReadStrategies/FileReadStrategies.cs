using lookwords.RxNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords
{
    public class FileReadStrategies
    {
        public static Dictionary<string, int> folderWordOccurrencesInSizeRangeSync(string folderPath, int minWordSize, int maxWordSize)
        {
            Dictionary<String, int> words_dict = new Dictionary<String, int>();
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);


            allfiles.ToList()
               .ForEach(async currFile =>
               {
                   IEnumerable<string> enume = FileUtils.getLinesSync(currFile, minWordSize, maxWordSize)
                           .TakeWhile(line => !line.Contains("*** END OF "))
                           .Select(line => new string(line.Where(c => !char.IsPunctuation(c)).ToArray()))
                           .SelectMany(line => line.Split(' ')
                                               .Where(word => !string.IsNullOrEmpty(word))
                                               .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize));
                   
                   
                   FileUtils.addWordToDictionary(enume, words_dict);
                           
               });

            //foreach (var curr in words_dict)
            //{
            //    Console.WriteLine("Palavra: " + curr.Key + " repetitions: " + curr.Value);
            //}
            return words_dict;
        }

        public static async Task<Dictionary<string, int>> folderWordOccurrencesInSizeRangeWithAsyncEnumerable(string folderPath, int minWordSize, int maxWordSize)
        {
            Dictionary<String, int> words_dict = new Dictionary<String, int>();
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);


            allfiles.ToList()
               .ForEach(async currFile =>
               {
                   await FileUtils.getLinesAsyncEnum(currFile, minWordSize, maxWordSize)
                            .TakeWhile(line => !line.Contains("*** END OF "))
                            .Select(line => new string(line.Where(c => !char.IsPunctuation(c)).ToArray()))
                            .Select(line => line.Split(' ')
                                                .Where(word => !string.IsNullOrEmpty(word))
                                                .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize))
                            .ForEachAsync(word => FileUtils.addWordToDictionary(word, words_dict));
               });

            foreach (var curr in words_dict)
            {
                Console.WriteLine("Palavra: " + curr.Key + " repetitions: " + curr.Value);
            }

            return words_dict;
        }

        //RXNET implementation
        public static async Task<Dictionary<string, int>> folderWordOccurrencesInSizeRangeWithRX(string folderPath, int minWordSize, int maxWordSize)
        {
            Console.WriteLine("Initiation: ");
            Dictionary<String, int> words_dict = new Dictionary<String, int>();
            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {
                FileTreeTextObservable observable = new FileTreeTextObservable(folderPath);
                WordObserver obs = new WordObserver(words_dict, minWordSize, maxWordSize);

                observable.Subscribe(obs);
                await observable.TakeWhile(line => !line.Contains("*** END OF "))
                           .Select(line => new string(line.Where(c => !char.IsPunctuation(c)).ToArray()))
                           .Select(line => line.Split(' ')
                                             .Where(word => !string.IsNullOrEmpty(word))
                                             .Where(word => word.Length >= minWordSize && word.Length <= maxWordSize))
                           .ForEachAsync(word => FileUtils.addWordToDictionary(word, words_dict));
            }



            //foreach (var curr in words_dict)
            //{
            //    Console.WriteLine("Palavra: " + curr.Key + " repetitions: " + curr.Value);
            //}

            return words_dict;
        }
    }
}
