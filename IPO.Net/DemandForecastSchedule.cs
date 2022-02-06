namespace IPO.Net {
    /// <summary>
    /// 수요예측 일정
    /// </summary>
    public class DemandForecastSchedule : DataRow {
        [DataColumn(0)]
        public string 종목명 { get; set; }
        /// <summary>
        /// 수요예측일 (EndAt의 년은 정확하지 않을 수 있습니다.)
        /// </summary>
        [DataColumn(1)]
        public DateTimeRange? 수요예측일 { get; set; }
        /// <summary>
        /// 희망공모가 (원)
        /// </summary>
        [DataColumn(2)]
        public LongRange? 희망공모가 { get; set; }
        /// <summary>
        /// 확정공모가 (원)
        /// </summary>
        [DataColumn(3)]
        public long? 확정공모가 { get; set; }
        /// <summary>
        /// 공모금액 (백만)
        /// </summary>
        [DataColumn(4)]
        public long? 공모금액 { get; set; }
        [DataColumn(5, convertParameter: ",Y")]
        public string[] 주간사 { get; set; }
    }
}
