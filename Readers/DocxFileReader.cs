using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace FileAnalyzer.Readers
{
    /// <summary>
    /// Reads .docx files without third-party libraries:
    /// a .docx is a zip archive whose text lives in word/document.xml.
    /// </summary>
    public class DocxFileReader : IFileReader
    {
        private const string DocumentEntryName = "word/document.xml";
        private static readonly XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        public IReadOnlyCollection<string> SupportedExtensions { get; } = new[] { ".docx" };

        public string Read(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    var entry = archive.GetEntry(DocumentEntryName);
                    if (entry == null)
                        throw new FileReadException("Geçersiz .docx dosyası: 'word/document.xml' bulunamadı.");

                    using (var entryStream = entry.Open())
                    {
                        return ExtractText(XDocument.Load(entryStream));
                    }
                }
            }
            catch (InvalidDataException ex)
            {
                throw new FileReadException("Dosya bozuk veya geçerli bir .docx dosyası değil.", ex);
            }
        }

        private static string ExtractText(XDocument document)
        {
            var builder = new StringBuilder();

            foreach (var paragraph in document.Descendants(W + "p"))
            {
                foreach (var element in paragraph.Descendants())
                {
                    if (element.Name == W + "t") builder.Append(element.Value);
                    else if (element.Name == W + "tab") builder.Append('\t');
                    else if (element.Name == W + "br" || element.Name == W + "cr") builder.Append('\n');
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
