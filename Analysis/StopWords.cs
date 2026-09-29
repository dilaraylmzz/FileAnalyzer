using System;
using System.Collections.Generic;

namespace FileAnalyzer.Analysis
{
    /// <summary>Conjunctions (Turkish + English) excluded from word-frequency results.</summary>
    public static class StopWords
    {
        public static readonly HashSet<string> Conjunctions = new HashSet<string>(StringComparer.Ordinal)
        {
            // Turkish
            "ve", "veya", "ya", "yahut", "veyahut", "ile", "ama", "fakat", "ancak", "lakin",
            "yalnız", "oysa", "oysaki", "halbuki", "hâlbuki", "çünkü", "zira", "ki", "de", "da",
            "hem", "yoksa", "ise", "gerek", "hatta", "üstelik", "dahası", "madem", "mademki",
            "eğer", "şayet", "yani", "ayrıca", "dolayısıyla", "böylece", "ne",

            // English
            "and", "or", "but", "nor", "so", "yet", "for", "because", "although", "though",
            "while", "whereas", "if", "unless", "since", "than", "either", "neither", "both",
            "whether", "as"
        };
    }
}
