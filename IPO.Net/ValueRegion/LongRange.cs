using System.Globalization;

namespace IPO.Net {
    public readonly struct LongRange : IFormattable {
        public LongRange(long startAt, long endAt) {
            if (startAt > endAt) throw new ArgumentException("시작이 끝보다 클 수 없습니다.");
            StartAt = startAt;
            EndAt = endAt;
        }

        public long StartAt { get; }
        public long EndAt { get; }

        public bool IsInner(long time) => time >= StartAt && time <= EndAt;

        public static LongRange Parse(string value, char splitChar, bool onlyDigit = false) {
            if (!TryParse(value, splitChar, out var p, onlyDigit)) throw new FormatException($"파싱 실패 ({value})");
            return p;
        }

        public static bool TryParse(string value, char splitChar, out LongRange price, bool onlyDigit = false) {
            price = default;
            var cs = value.Split(splitChar);
            bool a = false, b;
            b = long.TryParse(onlyDigit ? cs[0].GetDigitArray() : cs[0].Trim(), NumberStyles.Any, null, out var startAt);
            if (cs.Length < 2 || !(a = long.TryParse(onlyDigit ? cs[1].GetDigitArray() : cs[1].Trim(), NumberStyles.Any, null, out var endAt))) endAt = startAt;
            if (!a && !b) return false;

            if (endAt < startAt) (endAt, startAt) = (startAt, endAt);

            price = new(startAt, endAt);

            return true;
        }
        
        public override string ToString() => $"{StartAt}~{EndAt}";

        public string ToString(string format) =>
            string.Format(format, StartAt, EndAt);

        public string ToString(string format, IFormatProvider formatProvider) =>
            string.Format(format, StartAt, EndAt, formatProvider);
    }
}
