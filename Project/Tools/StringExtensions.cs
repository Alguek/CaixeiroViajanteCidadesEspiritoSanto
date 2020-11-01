using System.Globalization;
using System.Text;

namespace Project.Tools
{
    public static class StringExtensions
    {
        public static string RemoveAccents(this string text)
        {
            if (text == null)
                return null;

            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }

            return sbReturn.ToString();
        }
        
        public static string MergeSpaces(this string str)
        {

            if (str == null)
                return null;

            var stringBuilder = new StringBuilder(str.Length);

            var i = 0;
            foreach (var c in str)
            {
                if (c != ' ' || i == 0 || str[i - 1] != ' ')
                    stringBuilder.Append(c);
                i++;
            }
            return stringBuilder.ToString();

        }

        public static string LimparString(this string text)
        {
            text = text.Trim().RemoveAccents().MergeSpaces();
            return text;
        }
    }
}