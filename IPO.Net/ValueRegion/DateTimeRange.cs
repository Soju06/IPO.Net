using System.Globalization;

namespace IPO.Net {
    /// <summary>
    /// 날자 영역
    /// </summary>
    public readonly struct DateTimeRange : IFormattable {
        public DateTimeRange(DateTime startAt, DateTime endAt) {
            if (startAt > endAt) throw new ArgumentException("시작이 끝보다 클 수 없습니다.");
            StartAt = startAt;
            EndAt = endAt;
        }

        public static DateTimeRange BackToEnd(DateTime startAt) => new(startAt, DateTime.MaxValue);

        public DateTime StartAt { get; }
        public DateTime EndAt { get; }

        public bool IsInner(DateTime time) => time >= StartAt && time <= EndAt;

        public static DateTimeRange Parse(string value, string aFormat, string bFormat, char splitChar, IFormatProvider? provider = null, int dMode = 0) {
            if (TryParse(value, aFormat, bFormat, splitChar, provider ?? CultureInfo.CurrentCulture, dMode, out var v)) return v;
            throw new FormatException($"날자 파싱에 실패했습니다. ({value}) formatA: {aFormat}, formatB: {bFormat}, split: {splitChar}");
        }

        public static bool TryParse(string value, string aFormat, string bFormat, char splitChar, IFormatProvider provider, int dMode, out DateTimeRange date) {
            date = default;
            var cs = value.Split(splitChar);
            if (cs.Length < 2) return false;
            if (!DateTime.TryParseExact(cs[0].Trim(), aFormat, provider, DateTimeStyles.AllowWhiteSpaces, out var startAt)) return false;
            if (!DateTime.TryParseExact(cs[1].Trim(), bFormat, provider, DateTimeStyles.AllowWhiteSpaces, out var endAt)) return false;
            if (endAt < startAt) {
                switch (dMode) {
                    case 1:
                    endAt = endAt.AddYears(endAt.Year - startAt.Year + 1);
                    break;
                    case 0:
                    default:
                    (endAt, startAt) = (startAt, endAt);
                    break;
                }
            }

            date = new(startAt, endAt);
            return true;
        }

        public override string ToString() => $"{StartAt}~{EndAt}";

        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) {
            var fsP = -1;
            var fLen = format.Length;
            for (int i = 0; i < fLen; i++) {
                var c = format[i];
                if(c == '|') fsP = i;
            }

            if(fsP != -1) return $"{StartAt.ToString(format[0..fsP], formatProvider)}{EndAt.ToString(format[(fsP + 1)..], formatProvider)}";
            return ToString();
        }
    }
}
