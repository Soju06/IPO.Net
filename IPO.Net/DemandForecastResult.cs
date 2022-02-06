namespace IPO.Net {
    /// <summary>
    /// 수요예측결과
    /// </summary>
    public class DemandForecastResult : DataRow {
        [DataColumn(0)]
        public string 기업명 { get; set; }
        [DataColumn(1)]
        public DateTime? 예측일 { get; set; }
        /// <summary>
        /// 공모희망가 (원)
        /// </summary>
        [DataColumn(2)]
        public LongRange? 공모희망가 { get; set; }
        /// <summary>
        /// 공모가 (원)
        /// </summary>
        [DataColumn(3, customConvertFuncName: "long_dOnly")]
        public long 공모가 { get; set; }
        /// <summary>
        /// 공모금액 (백만)
        /// </summary>
        [DataColumn(4)]
        public long? 공모금액 { get; set; }
        /// <summary>
        /// 기관경쟁률 (N:1)
        /// </summary>
        [DataColumn(5, customConvertFuncName: "경쟁률")]
        public double? 기관경쟁률 { get; set; }
        /// <summary>
        /// 의무보유확약 (100%)
        /// </summary>
        [DataColumn(6, customConvertFuncName: "100%")]
        public double? 의무보유확약 { get; set; }
        [DataColumn(7, convertParameter: ",Y")]
        public string[] 주간사 { get; set; }
    }
}
