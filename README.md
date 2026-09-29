# MSE C# FileAnalyzer

.txt ve .docx dosyalarını analiz eden C# konsol uygulaması (.NET Framework 4.8).

## Çalıştırma
Visual Studio 2022 ile `FileAnalyzer.csproj` dosyasını açın ve F5 ile çalıştırın
(veya `dotnet run`). Açılan pencereden dosya seçin.

## Çıktılar
- Toplam / farklı kelime sayısı
- Tekrar eden kelimeler (çoktan aza; bağlaç ve sayılar hariç)
- Noktalama işaretleri ve sayıları
- `logs/` altında günlük log dosyası, `reports/` altında tam rapor

## Yeni dosya türü eklemek
1. `IFileReader` arayüzünü uygulayan bir sınıf yazın (`Readers/`).
2. `FileReaderFactory.CreateDefault()` içinde `Register` ile ekleyin.
PDF örneği için `Optional/PdfFileReader.cs.txt` dosyasına bakın.

## Proje yapısı
```
Readers/   IFileReader, TxtFileReader, DocxFileReader, FileReaderFactory, exceptions
Analysis/  TextAnalyzer, AnalysisResult, StopWords
Logging/   ILogger, FileLogger
UI/        FilePicker (OpenFileDialog), ReportBuilder
```

## GitHub
```
git init
git add .
git commit -m "Initial commit: FileAnalyzer with txt and docx support"
git branch -M main
git remote add origin https://github.com/<kullanici>/FileAnalyzer.git
git push -u origin main
```
