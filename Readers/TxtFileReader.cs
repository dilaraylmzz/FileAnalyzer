using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileAnalyzer.Readers
{
    public class TxtFileReader : IFileReader
    {
        public IReadOnlyCollection<string> SupportedExtensions { get; } = new[] { ".txt" };

        public string Read(string filePath)
        {
            // UTF-8 by default; BOM (UTF-8/UTF-16/UTF-32) is detected automatically.
            using (var reader = new StreamReader(filePath, Encoding.UTF8, detectEncodingFromByteOrderMarks: true))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
