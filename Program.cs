using System;
using System.IO;
using System.Text;
using FileAnalyzer.Analysis;
using FileAnalyzer.Logging;
using FileAnalyzer.Readers;
using FileAnalyzer.UI;

namespace FileAnalyzer
{
    internal static class Program
    {
        // OpenFileDialog requires an STA thread.
        [STAThread]
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.WriteLine("=== MSE C# FileAnalyzer ===");

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var logger = new FileLogger(Path.Combine(baseDirectory, "logs"));
            var readerFactory = FileReaderFactory.CreateDefault();
            var analyzer = new TextAnalyzer(StopWords.Conjunctions);
            var filePicker = new FilePicker(readerFactory.SupportedExtensions);

            var app = new FileAnalyzerApp(readerFactory, analyzer, filePicker, logger,
                                          Path.Combine(baseDirectory, "reports"));
            app.Run();
        }
    }
}
