using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.RxNet
{
    public class FileTreeTextObservable : IObservable<string>
    {
        private List<IObserver<string>> observers = new List<IObserver<string>>();
        string file;

        public FileTreeTextObservable( string file)
        {
            this.file = file;
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }

            readFileAsync(observer);

            return new Unsubscriber(observers, observer);
        }

        private async void readFileAsync( IObserver<string> observer)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        String line = null;
                        
                            //
                            // Not sure if there is any throughput gain by reading the file Asynchrnously.
                            // Test both: ReadLine() and ReadLineAsync() and check which one performs better??
                            //
                        line = await reader.ReadLineAsync();
                        observer.OnNext(line);
                    }
                    catch (Exception ex)
                    {
                        //
                        // Complete with error
                        //
                        observer.OnError(ex);
                    }
                }
                observer.OnCompleted();
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
