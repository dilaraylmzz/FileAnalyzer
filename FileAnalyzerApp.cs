using System;
using System.IO;
using System.Text;
using FileAnalyzer.Analysis;
using FileAnalyzer.Logging;
using FileAnalyzer.Readers;
using FileAnalyzer.UI;

namespace FileAnalyzer
{
    public class FileAnalyzerApp
    {
        private const int ConsoleRepeatedWordLimit = 30;

        private readonly FileReaderFactory _readerFactory;
        private readonly TextAnalyzer _analyzer;
        private readonly FilePicker _filePicker;
        private readonly ILogger _logger;
        private readonly string _reportDirectory;

        public FileAnalyzerApp(FileReaderFactory readerFactory, TextAnalyzer analyzer,
                               FilePicker filePicker, ILogger logger, string reportDirectory)
        {
            _readerFactory = readerFactory;
            _analyzer = analyzer;
            _filePicker = filePicker;
            _logger = logger;
            _reportDirectory = reportDirectory;
        }

        public void Run()
        {
            _logger.Info("Uygulama başlatıldı.");

            while (true)
            {
                string filePath = GetFilePathFromUser();
                if (filePath == null) break;

                ProcessFile(filePath);

                Console.Write("\nBaşka bir dosya analiz etmek ister misiniz? (e/h): ");
                string answer = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (answer != "e" && answer != "evet") break;
            }

            _logger.Info("Uygulama kapatıldı.");
        }

        /// <summary>Opens the dialog; if cancelled, falls back to manual path entry. Null = exit.</summary>
        private string GetFilePathFromUser()
        {
            Console.WriteLine("\nDosya seçim penceresi açılıyor...");
            string path = _filePicker.PickFile();
            if (path != null) return path;

            Console.Write("Dosya seçilmedi. Dosya yolunu elle girin (çıkmak için boş bırakın): ");
            string manual = Console.ReadLine()?.Trim().Trim('"');
            return string.IsNullOrEmpty(manual) ? null : manual;
        }

        private void ProcessFile(string filePath)
        {
            _logger.Info($"Dosya işleniyor: {filePath}");

            try
            {
                ValidatePath(filePath);

                IFileReader reader = _readerFactory.GetReader(filePath);
                string text = reader.Read(filePath);
                _logger.Info($"Dosya okundu ({text.Length} karakter).");

                AnalysisResult result = _analyzer.Analyze(text);
                _logger.Info($"Analiz tamamlandı: {result.TotalWordCount} kelime, {result.DistinctWordCount} farklı kelime.");

                Console.WriteLine();
                Console.WriteLine(ReportBuilder.Build(filePath, result, ConsoleRepeatedWordLimit));
                SaveReport(filePath, result);
            }
            catch (FileNotFoundException ex)
            {
                ShowError("Dosya bulunamadı: " + filePath, ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                ShowError("Klasör yolu geçersiz: " + filePath, ex);
            }
            catch (ArgumentException ex)
            {
                ShowError("Dosya yolu geçersiz karakterler içeriyor.", ex);
            }
            catch (UnsupportedFileTypeException ex)
            {
                ShowError(ex.Message, ex);
            }
            catch (FileReadException ex)
            {
                ShowError(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError("Dosyaya erişim izni yok.", ex);
            }
            catch (IOException ex)
            {
                ShowError("Dosya okunurken bir G/Ç hatası oluştu (dosya başka bir program tarafından kullanılıyor olabilir).", ex);
            }
            catch (Exception ex)
            {
                ShowError("Beklenmeyen bir hata oluştu.", ex);
            }
        }

        private static void ValidatePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Dosya yolu boş olamaz.");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Dosya bulunamadı.", filePath);
        }

        private void SaveReport(string filePath, AnalysisResult result)
        {
            try
            {
                Directory.CreateDirectory(_reportDirectory);
                string sourceExtension = Path.GetExtension(filePath).TrimStart('.').ToLowerInvariant();
                string reportName = $"report_{Path.GetFileNameWithoutExtension(filePath)}_{sourceExtension}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string reportPath = Path.Combine(_reportDirectory, reportName);

                File.WriteAllText(reportPath, ReportBuilder.Build(filePath, result), Encoding.UTF8);
                Console.WriteLine($"Tam rapor kaydedildi: {reportPath}");
                _logger.Info($"Rapor kaydedildi: {reportPath}");
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                _logger.Warning("Rapor dosyası kaydedilemedi: " + ex.Message);
                Console.WriteLine("Uyarı: Rapor dosyası kaydedilemedi.");
            }
        }

        private void ShowError(string userMessage, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("HATA: " + userMessage);
            Console.ResetColor();
            _logger.Error(userMessage, ex);
        }
    }
}
