using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace lookwords
{
    public class AsyncFiles
    {

        public static Channel<string> readFileAllLinesAsync(string  filename)
        {
            var lineChannel = Channel.CreateUnbounded<string>();
            Task.Run(async () =>
            {
                using var fileHandle = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                var buffer = new byte[4096];
                string partialLastLineFromPreviousBuffer = null;

                while (true)
                {
                    int bytesRead = await fileHandle.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        break;
                    }

                    string content = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    string[] lines = content.Split('\n');

                    if (partialLastLineFromPreviousBuffer != null)
                    {
                        lines[0] = partialLastLineFromPreviousBuffer + lines[0];
                        partialLastLineFromPreviousBuffer = null;
                    }

                    if (content[content.Length - 1] != '\n')
                    {
                        partialLastLineFromPreviousBuffer = lines[lines.Length - 1];
                        Array.Resize(ref lines, lines.Length - 1);
                    }

                    foreach (var line in lines)
                    {
                        await lineChannel.Writer.WriteAsync(line);
                    }
                }

                lineChannel.Writer.Complete();
            });

            return lineChannel;

        }

        public static IObservable<string> GestObservableReadFileAsync(string filePath)
        {
            return Observable.Create<string>(async observer =>
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = await reader.ReadLineAsync();
                            observer.OnNext(line);
                        }
                    }
                    observer.OnCompleted();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        }
    }
}
