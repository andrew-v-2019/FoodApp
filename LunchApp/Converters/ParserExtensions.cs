using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Spire.Doc.Interface;

namespace Converters
{
    public static class ParserExtensions
    {
        private const string DateFormat = "dd.MM.yyyy";
        private const string DateRegEx = @"(\d+)[-.\/](\d+)[-.\/](\d+)";
        private const string DoubleRegEx = @"[0-9]{1,13}(\\.[0-9]*)?";
        private const string IntRegEx = @"\b(?<!\.)\d+(?!\.)\b";

        public static DateTime? ExtractDateFromTitle(this string s)
        {
            var r = new Regex(DateRegEx);
            var m = r.Match(s);
            if (!m.Success) return null;
            var dt = DateTime.ParseExact(m.Value, DateFormat, CultureInfo.InvariantCulture);
            return dt;
        }

        public static string RemoveNumber(this string s)
        {
            return !s.IsLineWithNum() ? s : s.Remove(0, 2).Trim();
        }

        public static bool IsLineWithNum(this string s)
        {
            var isint = s.Trim()[0].ToString().IsInt();
            var isdot = s.Trim()[1] == '.';
            return isint && isdot;
        }

        public static double? ExtractPriceFromString(this string s)
        {
            var r = new Regex(DoubleRegEx);
            var m = r.Match(s);
            if (!m.Success) return null;
            var n = Convert.ToDouble(m.Value);
            return n;
        }

        public static bool IsInt(this string s)
        {
            var r = new Regex(IntRegEx);
            var m = r.Match(s);
            return m.Success;
        }

    }
}
