using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.RxNet
{
    public class FolderWordsObservable : IObservable<string>
    {
        private List<IObserver<string>> observers = new List<IObserver<string>>();
        string folderPath;
        private readonly int minLength;
        private readonly int maxLength;

        public FolderWordsObservable( string folderPath, int minLength, int maxLength)
        {
            this.folderPath = folderPath;
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            
                IAsyncEnumerable<string> enume = fileUtils.FileUtils.Lines(folderPath);

                Console.WriteLine("Initiation: ");


                string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

                foreach (var file in allfiles)
                {
                    using (StreamReader reader = new StreamReader(file))

                        while (!reader.EndOfStream)
                        {
                            _ = reader.ReadLineAsync()
                                .ContinueWith(line => processLineAndNotifyWordsToObserver(line.Result, observer));
                        }
                }
            }

            return new Unsubscriber(observers, observer);
        }

        private void processLineAndNotifyWordsToObserver(string line, IObserver<string> observer)
        {
            if (line.Contains("*** END OF ") || String.IsNullOrEmpty(line))
            {
                observer.OnCompleted();
                return;
            }
            else
            {
                String[] words = line.Split(' ');

                try
                {
                    foreach (var word in words.AsEnumerable())
                    {
                        if (word.Length > minLength && word.Length < maxLength)
                        { 
                            observer.OnNext(word);
                        }
                    }
                } catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            }
        }
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<string>> _observers;
            private IObserver<string> _observer;

            public Unsubscriber(List<IObserver<string>> observers, IObserver<string> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
