using System.Linq;
using System.Text;
using FileAnalyzer.Analysis;

namespace FileAnalyzer.UI
{
    public static class ReportBuilder
    {
        public static string Build(string filePath, AnalysisResult result, int maxRepeatedWords = int.MaxValue)
        {
            var sb = new StringBuilder();
            sb.AppendLine("================ DOSYA ANALİZ RAPORU ================");
            sb.AppendLine($"Dosya                          : {filePath}");
            sb.AppendLine($"Toplam kelime sayısı           : {result.TotalWordCount}");
            sb.AppendLine($"Toplam farklı kelime sayısı    : {result.DistinctWordCount}");
            sb.AppendLine($"Farklı kelime (bağlaçsız)      : {result.DistinctWordCountWithoutConjunctions}");
            sb.AppendLine();

            sb.AppendLine("--- Tekrar eden kelimeler (bağlaç ve sayılar hariç) ---");
            if (result.RepeatedWords.Count == 0)
            {
                sb.AppendLine("Tekrar eden kelime bulunamadı.");
            }
            else
            {
                foreach (var item in result.RepeatedWords.Take(maxRepeatedWords))
                    sb.AppendLine($"{item.Word,-25} {item.Count}");

                int hidden = result.RepeatedWords.Count - maxRepeatedWords;
                if (hidden > 0)
                    sb.AppendLine($"... ve {hidden} kelime daha (tam liste rapor dosyasında).");
            }
            sb.AppendLine();

            sb.AppendLine("--- Noktalama işaretleri ---");
            sb.AppendLine($"Toplam: {result.TotalPunctuationCount}");
            foreach (var p in result.PunctuationCounts)
                sb.AppendLine($"'{p.Key}'  : {p.Value}");
            sb.AppendLine("=====================================================");

            return sb.ToString();
        }
    }
}
