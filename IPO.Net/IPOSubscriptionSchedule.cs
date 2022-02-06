using System.Web;

namespace IPO.Net {
    /// <summary>
    /// 공모청약일정
    /// </summary>
    public class IPOSubscriptionSchedule : DataRow {
        [DataColumn(0)]
        public string 종목명 { get; set; }
        [DataColumn(1)]
        public DateTimeRange 공모주일정 { get; set; }
        /// <summary>
        /// 확정공모가 (원)
        /// </summary>
        [DataColumn(2)]
        public long? 확정공모가 { get; set; }
        [DataColumn(3)]
        public LongRange 희망공모가 { get; set; }
        /// <summary>
        /// 청약경쟁률 (N:1)
        /// </summary>
        [DataColumn(4, customConvertFuncName: "경쟁률")]
        public double? 청약경쟁률 { get; set; }
        [DataColumn(5, convertParameter: ",Y")]
        public string[] 주간사 { get; set; }

        [DataColumn(6)]
        public int 분석Id => 분석id;

        int 분석id;

        [DataColumn(6)]
        public HtmlNode _분석 {
            set {
                var n = value.SelectSingleNode("./a[@href]");
                var href = n?.GetAttributeValue("href", null).HtmlDecode();
                if (n == null || href == null) throw new FormatException("분석 컬럼을 찾을 수 없습니다.");
                분석id = int.Parse(HttpUtility.ParseQueryString(new Uri(new Uri("https://www.38.co.kr/"), href).Query).Get("no"));
            } 
        }
    }
}
