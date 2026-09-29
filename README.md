# MSE C# FileAnalyzer

`.txt`, `.docx` ve `.pdf` dosyalarını analiz eden C# konsol uygulaması (.NET Framework 4.8).

## Çalıştırma

Visual Studio 2022 ile `FileAnalyzer.csproj` dosyasını açın ve **Ctrl + F5** ile çalıştırın.
Açılan pencereden bir dosya seçin. Pencere iptal edilirse dosya yolu elle de girilebilir.

## Çıktılar

- Toplam kelime sayısı ve toplam farklı kelime sayısı
- Tekrar eden kelimeler (çoktan aza sıralı; bağlaçlar ve sayılar hariç)
- Noktalama işaretleri ve sayıları
- `logs/` klasöründe günlük log dosyası
- `reports/` klasöründe tam rapor (`report_<dosya>_<uzanti>_<tarih>.txt` biçiminde)

Klasörler `bin/Debug/net48/` altında oluşur.

## Desteklenen dosya türleri

| Uzantı | Okuyucu |
|---|---|
| `.txt` | `TxtFileReader` |
| `.docx` | `DocxFileReader` (harici kütüphane kullanmaz) |
| `.pdf` | `PdfFileReader` (PdfPig paketi) |

## Hata yönetimi

Geçersiz yol, desteklenmeyen uzantı, bozuk dosya ve erişim hataları yakalanır,
kullanıcıya anlaşılır bir mesajla iletilir ve log dosyasına yazılır.

## Yeni dosya türü eklemek

1. `Readers/` altında `IFileReader` arayüzünü uygulayan bir sınıf yazın.
2. `FileReaderFactory.CreateDefault()` içinde `Register` ile ekleyin.

## Proje yapısı

```
Readers/   IFileReader, TxtFileReader, DocxFileReader, PdfFileReader,
           FileReaderFactory, exceptions
Analysis/  TextAnalyzer, AnalysisResult, StopWords
Logging/   ILogger, FileLogger
UI/        FilePicker (OpenFileDialog), ReportBuilder
```