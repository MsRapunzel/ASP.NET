namespace labWork.Logging
{
    public class FileLogger : ILogger, IDisposable
    {
        string filePath;

        static object _lock = new object();

        public FileLogger(string path)
        {

            filePath = path;
        }

        public IDisposable BeginScope<TState>(TState state) => this;

        public void Dispose() { }

        public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel.Error;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (_lock)
            {
                if (IsEnabled(logLevel))
                {
                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}