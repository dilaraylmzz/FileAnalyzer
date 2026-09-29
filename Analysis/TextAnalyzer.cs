using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileAnalyzer.Analysis
{
    public class TextAnalyzer
    {
        private const int MinWordLength = 2;

        // Only letters form words, so numbers ("2024", "3.14") are ignored automatically.
        private static readonly Regex WordRegex = new Regex(@"\p{L}+", RegexOptions.Compiled);
        private static readonly CultureInfo TurkishCulture = new CultureInfo("tr-TR");

        private readonly ISet<string> _conjunctions;

        public TextAnalyzer(ISet<string> conjunctions)
        {
            _conjunctions = conjunctions ?? throw new ArgumentNullException(nameof(conjunctions));
        }

        public AnalysisResult Analyze(string text)
        {
            text = text ?? string.Empty;

            var words = ExtractWords(text);
            var filteredWords = words.Where(w => !_conjunctions.Contains(w)).ToList();

            var repeatedWords = filteredWords
                .GroupBy(w => w)
                .Where(g => g.Count() > 1)
                .Select(g => new WordFrequency { Word = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Word, StringComparer.Create(TurkishCulture, false))
                .ToList();

            var punctuation = CountPunctuation(text);

            return new AnalysisResult
            {
                TotalWordCount = words.Count,
                DistinctWordCount = words.Distinct().Count(),
                DistinctWordCountWithoutConjunctions = filteredWords.Distinct().Count(),
                RepeatedWords = repeatedWords,
                PunctuationCounts = punctuation,
                TotalPunctuationCount = punctuation.Sum(p => p.Value)
            };
        }

        private static List<string> ExtractWords(string text)
        {
            return WordRegex.Matches(text)
                .Cast<Match>()
                .Select(m => m.Value.ToLower(TurkishCulture))
                .Where(w => w.Length >= MinWordLength)
                .ToList();
        }

        private static List<KeyValuePair<char, int>> CountPunctuation(string text)
        {
            return text.Where(char.IsPunctuation)
                .GroupBy(c => c)
                .Select(g => new KeyValuePair<char, int>(g.Key, g.Count()))
                .OrderByDescending(p => p.Value)
                .ThenBy(p => p.Key)
                .ToList();
        }
    }
}
