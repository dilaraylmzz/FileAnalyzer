using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileAnalyzer.Readers
{
    /// <summary>
    /// Picks the correct reader by file extension.
    /// Extensibility point: add new readers in <see cref="CreateDefault"/>.
    /// </summary>
    public class FileReaderFactory
    {
        private readonly List<IFileReader> _readers = new List<IFileReader>();

        public static FileReaderFactory CreateDefault()
        {
            var factory = new FileReaderFactory();
            factory.Register(new TxtFileReader());
            factory.Register(new DocxFileReader());
            factory.Register(new PdfFileReader()); // Optional: see Optional/PdfFileReader.cs.txt
            return factory;
        }

        public void Register(IFileReader reader) => _readers.Add(reader);

        public IEnumerable<string> SupportedExtensions =>
            _readers.SelectMany(r => r.SupportedExtensions).Distinct(StringComparer.OrdinalIgnoreCase);

        public IFileReader GetReader(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            var reader = _readers.FirstOrDefault(r =>
                r.SupportedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase));

            if (reader == null)
            {
                throw new UnsupportedFileTypeException(
                    $"'{extension}' dosya türü desteklenmiyor. Desteklenenler: {string.Join(", ", SupportedExtensions)}");
            }
            return reader;
        }
    }
}
