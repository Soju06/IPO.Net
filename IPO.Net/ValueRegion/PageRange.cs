namespace IPO.Net {
    /// <summary>
    /// 페이지 영역
    /// </summary>
    public readonly struct PageRange : IEnumerable<int> {
        /// <summary>
        /// 시작 페이지
        /// </summary>
        public static readonly PageRange StartPage = new(0);

        /// <summary>
        /// <paramref name="offset"/>에서 끝까지
        /// </summary>
        /// <param name="offset">시작점</param>
        public static PageRange BackToEnd(int offset = 0) => new(offset, -1);

        public PageRange(int offset, int count = 1) {
            Offset = offset;
            if (count < 0) {
                Count = 1;
                IsInfinite = true;
            } else {
                Count = (uint)count;
                IsInfinite = false;
            }
        }

        /// <summary>
        /// 시작
        /// </summary>
        public int Offset { get; }
        /// <summary>
        /// 갯수
        /// </summary>
        public uint Count { get; }

        public bool IsInfinite { get; }

        public void Deconstruct(out int offset, out uint count) {
            offset = Offset;
            count = Count;
        }

        public IEnumerator<int> GetEnumerator() {
            for (int i = 0; IsInfinite || i < Count; i++)
                yield return Offset + i;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
