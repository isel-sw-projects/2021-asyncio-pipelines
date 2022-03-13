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

        public FolderWordsObservable( string folderPath)
        {
            this.folderPath = folderPath;
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }

            countWords(observer).Wait();

            return new Unsubscriber(observers, observer);
        }

        private async Task countWords( IObserver<string> observer)
        {
            Console.WriteLine("Initiation: ");


            string[] allfiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {

                using (StreamReader reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            string line = await reader.ReadLineAsync();

                            observer.OnNext(line);

                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                        }

                    }
                }
            }    
            
            observer.OnCompleted();
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
