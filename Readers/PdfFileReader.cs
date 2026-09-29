using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UglyToad.PdfPig;

namespace FileAnalyzer.Readers
{
    public class PdfFileReader : IFileReader
    {
        public IReadOnlyCollection<string> SupportedExtensions { get; } = new[] { ".pdf" };

        public string Read(string filePath)
        {
            try
            {
                var builder = new StringBuilder();
                using (var document = PdfDocument.Open(filePath))
                {
                    foreach (var page in document.GetPages())
                        builder.AppendLine(page.Text);
                }
                return builder.ToString();
            }
            catch (Exception ex) when (!(ex is IOException) && !(ex is UnauthorizedAccessException))
            {
                throw new FileReadException(
                    "PDF okunamadı. Dosya bozuk, şifreli veya geçerli bir PDF olmayabilir.", ex);
            }
        }
    }
}
