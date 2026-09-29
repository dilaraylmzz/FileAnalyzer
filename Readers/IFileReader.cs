using System.Collections.Generic;

namespace FileAnalyzer.Readers
{
    /// <summary>
    /// Contract for every file type reader. To support a new file type,
    /// implement this interface and register it in <see cref="FileReaderFactory"/>.
    /// </summary>
    public interface IFileReader
    {
        /// <summary>Supported extensions including the dot, e.g. ".txt".</summary>
        IReadOnlyCollection<string> SupportedExtensions { get; }

        /// <summary>Reads the file and returns its plain text content.</summary>
        string Read(string filePath);
    }
}
