using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ERNIE_Bot.SDK
{
    /// <summary>
    /// This class provides methods for tokenizing text.
    /// </summary>
    public static class Tokenizer
    {
        public static int ApproxNumTokens(string text)
        {
            int chinese = Regex.Matches(text, @"\p{IsCJKUnifiedIdeographs}").Count;
            int english = Regex.Replace(text, @"[^\p{IsBasicLatin}-]", " ")
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Count(w => !string.IsNullOrWhiteSpace(w) && w != "-" && w != "_");

            return chinese + (int)Math.Floor(english * 1.3);
        }
    }
}
