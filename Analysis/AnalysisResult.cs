using System.Collections.Generic;

namespace FileAnalyzer.Analysis
{
    public class WordFrequency
    {
        public string Word { get; set; }
        public int Count { get; set; }
    }

    public class AnalysisResult
    {
        /// <summary>Every word in the text (numbers excluded).</summary>
        public int TotalWordCount { get; set; }

        /// <summary>Distinct words including conjunctions.</summary>
        public int DistinctWordCount { get; set; }

        /// <summary>Distinct words after removing conjunctions.</summary>
        public int DistinctWordCountWithoutConjunctions { get; set; }

        /// <summary>Repeated words (count &gt; 1), most frequent first, conjunctions excluded.</summary>
        public IReadOnlyList<WordFrequency> RepeatedWords { get; set; }

        /// <summary>Punctuation character -> occurrence count, most frequent first.</summary>
        public IReadOnlyList<KeyValuePair<char, int>> PunctuationCounts { get; set; }

        public int TotalPunctuationCount { get; set; }
    }
}
