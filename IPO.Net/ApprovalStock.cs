namespace IPO.Net {
    /// <summary>
    /// 승인 종목
    /// </summary>
    public class ApprovalStock : DataRow {
        [DataColumn(0)]
        public DateTime 승인일 { get; set; }
        [DataColumn(1)]
        public string 기업명 { get; set; }
        /// <summary>
        /// 년은 읽지 않습니다. (월/일)
        /// </summary>
        [DataColumn(2, customConvertFuncName: "date_mm_dd")]
        public DateTime 청구일 { get; set; }
        /// <summary>
        /// 자본금 (백만)
        /// </summary>
        [DataColumn(3)]
        public long 자본금 { get; set; }
        /// <summary>
        /// 매출액 (백만)
        /// </summary>
        [DataColumn(4)]
        public long 매출액 { get; set; }
        /// <summary>
        /// 당기순이익 (백만)
        /// </summary>
        [DataColumn(5)]
        public long 당기순이익 { get; set; }
        [DataColumn(6, convertParameter: ",Y")]
        public string[] 주간사 { get; set; }
        [DataColumn(7)]
        public string 주업종 { get; set; }
    }
}
