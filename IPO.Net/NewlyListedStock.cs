namespace IPO.Net {
    /// <summary>
    /// 신규상장
    /// </summary>
    public class NewlyListedStock : DataRow {
        [DataColumn(0)]
        public string 기업명 { get; set; }
        [DataColumn(1)]
        public DateTime 신규상장일 { get; set; }
        /// <summary>
        /// 현재가 (원)
        /// </summary>
        [DataColumn(2)]
        public long? 현재가 { get; set; }
        /// <summary>
        /// 전일비 (100%)
        /// </summary>
        [DataColumn(3, customConvertFuncName: "100%")]
        public double? 전일비 { get; set; }
        /// <summary>
        /// 공모가 (원)
        /// </summary>
        [DataColumn(4)]
        public long? 공모가 { get; set; }
        /// <summary>
        /// 공모가대비등락률 (100%)
        /// </summary>
        [DataColumn(5, customConvertFuncName: "100%")]
        public double? 공모가대비등락률 { get; set; }
        /// <summary>
        /// 시초가 (원)
        /// </summary>
        [DataColumn(6)]
        public long? 시초가 { get; set; }
        /// <summary>
        /// 시초/공모 (100%)
        /// </summary>
        [DataColumn(7, customConvertFuncName: "100%")]
        public double? 공모대비시초가 { get; set; }
        /// <summary>
        /// 첫날 종가 (원)
        /// </summary>
        [DataColumn(8)]
        public long? 첫날종가 { get; set; }
    }
}
