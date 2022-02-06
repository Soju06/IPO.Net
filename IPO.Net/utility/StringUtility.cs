using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IPO.Net.utility {
    internal static class StringUtility {
        public static char[] GetDigitArray(this string str) => str.Where(c => char.IsDigit(c)).ToArray();
        public static string GetDigitOnly(this string str) => new(GetDigitArray(str));
        public static int ParseInt32(this string str, bool digitOnly = false) =>
            int.Parse(digitOnly ? GetDigitArray(str) : str, NumberStyles.Any);
        public static long ParseInt64(this string str, bool digitOnly = false) =>
            long.Parse(digitOnly ? GetDigitArray(str) : str, NumberStyles.Any);
        public static double ParseDouble(this string str, bool digitOnly = false) =>
            double.Parse(digitOnly ? GetDigitArray(str) : str, NumberStyles.Any);
        public static bool TryParseInt32(this string str, out int value, bool digitOnly = false) =>
            int.TryParse(digitOnly ? GetDigitArray(str) : str, NumberStyles.Any, null, out value);
        public static bool TryParseInt64(this string str, out long value, bool digitOnly = false) =>
            long.TryParse(digitOnly ? GetDigitArray(str) : str, NumberStyles.Any, null, out value);
        public static bool TryParseDouble(this string str, out double value, bool digitOnly = false) =>
            double.TryParse(digitOnly ? GetDigitArray(str) : str, NumberStyles.Any, null, out value);
    }
}
