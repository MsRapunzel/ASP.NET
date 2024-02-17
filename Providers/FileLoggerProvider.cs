using System;
using labWork.Logging;

namespace labWork.Providers
{
    public class FileLoggerProvider : ILoggerProvider
    {
        string path;
        public FileLoggerProvider(string path)
        {
            this.path = path;
        }
        public ILogger CreateLogger(string categoryName) => new FileLogger(path);

        public void Dispose() { }
    }
}

