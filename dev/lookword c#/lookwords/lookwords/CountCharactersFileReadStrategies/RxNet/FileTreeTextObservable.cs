using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.CountCharactersStrategies.RxNet
{
    public class FileTreeCharacterObservable : IObservable<char>
    {
        private List<IObserver<char>> observers = new List<IObserver<char>>();
        string file;

        public FileTreeCharacterObservable( string file)
        {
            this.file = file;
        }

        public IDisposable Subscribe(IObserver<char> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }

            readFileAsync(observer);

            return new Unsubscriber(observers, observer);
        }

        private void readFileAsync( IObserver<char> observer)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        //
                        // Not sure if there is any throughput gain by reading the file Asynchrnously.
                        // Test bith: ReadLine() and ReadLineAsync() and check which one performs better??
                        //
                        string words = reader.ReadLine();

                        foreach (char c in words) { observer.OnNext(c); }

                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                    }
                }
            }    
            
            observer.OnCompleted();
        }
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<char>> _observers;
            private IObserver<char> _observer;

            public Unsubscriber(List<IObserver<char>> observers, IObserver<char> observer)
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
