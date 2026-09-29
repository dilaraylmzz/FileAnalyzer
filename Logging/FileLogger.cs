using System;
using System.IO;
using System.Text;

namespace FileAnalyzer.Logging
{
    /// <summary>Appends log lines to logs/log_yyyyMMdd.txt. Logging never crashes the app.</summary>
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;
        private readonly object _lock = new object();

        public FileLogger(string directory)
        {
            Directory.CreateDirectory(directory);
            _logFilePath = Path.Combine(directory, $"log_{DateTime.Now:yyyyMMdd}.txt");
        }

        public void Info(string message) => Write("INFO", message);
        public void Warning(string message) => Write("WARN", message);

        public void Error(string message, Exception exception = null) =>
            Write("ERROR", exception == null ? message : $"{message} | {exception.GetType().Name}: {exception.Message}");

        private void Write(string level, string message)
        {
            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}";
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(_logFilePath, line, Encoding.UTF8);
                }
            }
            catch (IOException) { /* logging must not break the application */ }
            catch (UnauthorizedAccessException) { }
        }
    }
}
