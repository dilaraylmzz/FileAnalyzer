using System;

namespace FileAnalyzer.Readers
{
    /// <summary>Thrown when a file cannot be read or is corrupted.</summary>
    public class FileReadException : Exception
    {
        public FileReadException(string message) : base(message) { }
        public FileReadException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>Thrown when no reader is registered for the file extension.</summary>
    public class UnsupportedFileTypeException : Exception
    {
        public UnsupportedFileTypeException(string message) : base(message) { }
    }
}
