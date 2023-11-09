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
            var hanCount = 0;
            var res = new StringBuilder(text.Length);

            foreach (var c in text)
            {
                if (char.IsWhiteSpace(c))
                {
                    res.Append(' ');
                }
                else if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    hanCount++;
                    res.Append(' ');
                }
                else
                {
                    res.Append(c);
                }
            }

            var wordCount = res.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            return hanCount + (int)Math.Floor(wordCount * 1.3);
        }
    }
}
