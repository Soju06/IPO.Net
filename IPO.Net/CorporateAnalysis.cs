using System.Globalization;

namespace IPO.Net {
    /// <summary>
    /// 기업 분석
    /// </summary>
    public class CorporateAnalysis : DataTable<DataRow> {
        public 기업개요? 개요 => GetOrNull(0) as 기업개요;
        public 공모정보? 공모 => GetOrNull(1) as 공모정보;
        public 인수회사? 인수 => GetOrNull(2) as 인수회사;
        public 청약일정? 청약 => GetOrNull(2) as 청약일정;

        protected DataRow? GetOrNull(int index) => index >= 0 && Count < index ? Row(index) : null;

        protected void Set(int index, DataRow? row) {
            if (Count <= index) {
                while (Count != index)
                    Add(null);
                Add(row);
            } else this[index] = row;
        }

        public void Parse(HtmlDocument document) {
            Clear();
            var a1_node = document.DocumentNode.SelectSingleNode("//table[@summary='기업개요']");
            if (a1_node == null) throw new NodeNotFoundException("분석이 없거나 잘못된 id입니다.");
            기업개요 a1 = new();
            a1.Parse(a1_node);
            Set(0, a1);

            공모정보 a2 = new();
            a2.Parse(GetXPaths(document, "//table[@summary='공모정보']"));
            Set(1, a2);

            인수회사 a3 = new();
            var a3_xp = GetXPaths(document, "//table[@summary='공모정보']/following-sibling::table");
            var a3_head = a3_xp?.GetElements()[0].GetElements();
            if (a3_head.Length >= 4 && a3_head[0].InnerText.Trim() == "인수회사" && a3_head[2].InnerText.Trim() == "청약한도")
                a3.Parse(a3_xp);
            Set(2, a3);

            청약일정 a4 = new();
            a4.Parse(GetXPaths(document, "//table[@summary='공모청약일정']"));
            Set(3, a4);
        }

        static HtmlNode GetXPaths(HtmlDocument document, string xpath) {
            var nd = document.DocumentNode.SelectSingleNode(xpath);
            if (nd == null) throw new FormatException($"올바르지 않은 노드입니다. ({xpath})");
            return nd;
        }

        public class 기업개요 : DataRow {
            string _종목명, _진행상황, _시장구분, _종목코드, _업종, _대표자, _기업구분, _본점소재지, _홈페이지, _대표전화, _최대주주;
            long _법인세비용차감전계속사업이익, _자본금;
            long? _매출액, _순이익;

            [DataColumn(0)]
            public string 종목명 => _종목명;
            [DataColumn(1)]
            public string 진행상황 => _진행상황;
            [DataColumn(2)]
            public string 시장구분 => _시장구분;
            [DataColumn(3)]
            public string 종목코드 => _종목코드;
            [DataColumn(4)]
            public string 업종 => _업종;
            [DataColumn(5)]
            public string 대표자 => _대표자;
            [DataColumn(6)]
            public string 기업구분 => _기업구분;
            [DataColumn(7)]
            public string 본점소재지 => _본점소재지;
            [DataColumn(8)]
            public string 홈페이지 => _홈페이지;
            [DataColumn(9)]
            public string 대표전화 => _대표전화;
            [DataColumn(10)]
            public string 최대주주 => _최대주주;
            /// <summary>
            /// 매출액 (백만원)
            /// </summary>
            [DataColumn(11)]
            public long? 매출액 => _매출액;
            /// <summary>
            /// 법인세비용차감전계속사업이익 (백만원)
            /// </summary>
            [DataColumn(12)]
            public long 법인세비용차감전계속사업이익 => _법인세비용차감전계속사업이익;
            /// <summary>
            /// 순이익 (백만원)
            /// </summary>
            [DataColumn(13)]
            public long? 순이익 => _순이익;
            /// <summary>
            /// 자본금 (백만원)
            /// </summary>
            [DataColumn(14)]
            public long 자본금 => _자본금;

            [DataColumn(0)]
            public HtmlNode 종목명_진행상황 {
                set {
                    var nodes = value.GetElements();
                    _종목명 = nodes[1].InnerText.HtmlDecode().Trim();
                    _진행상황 = nodes[3].InnerText.HtmlDecode().Trim();
                }
            }

            [DataColumn(1)]
            public HtmlNode 시장구분_총목코드 {
                set {
                    var nodes = value.GetElements();
                    _시장구분 = nodes[1].InnerText.HtmlDecode().Trim();
                    _종목코드 = nodes[3].InnerText.HtmlDecode().Trim();
                }
            }

            [DataColumn(2)]
            public HtmlNode 업종_ {
                set {
                    var nodes = value.GetElements();
                    _업종 = nodes[1].InnerText.HtmlDecode().Trim();
                }
            }

            [DataColumn(3)]
            public HtmlNode 대표자_기업구분 {
                set {
                    var nodes = value.GetElements();
                    _대표자 = nodes[1].InnerText.HtmlDecode().Trim();
                    _기업구분 = nodes[3].InnerText.HtmlDecode().Trim();
                }
            }

            [DataColumn(4)]
            public HtmlNode 본점소재지_ {
                set {
                    var nodes = value.GetElements();
                    _본점소재지 = nodes[1].InnerText.HtmlDecode().Trim();
                }
            }

            [DataColumn(5)]
            public HtmlNode 홈페이지_대표전화 {
                set {
                    var nodes = value.GetElements();
                    _홈페이지 = nodes[1].InnerText.HtmlDecode().Trim();
                    _대표전화 = nodes[3].InnerText.HtmlDecode().Trim();
                }
            }

            [DataColumn(6)]
            public HtmlNode 최대주주_ {
                set {
                    var nodes = value.GetElements();
                    _최대주주 = nodes[1].InnerText.HtmlDecode().Trim();
                }
            }

            [DataColumn(7)]
            public HtmlNode 매출액_법인세비용차감전계속사업이익 {
                set {
                    var nodes = value.GetElements();
                    if (nodes[1].InnerText.HtmlDecode().TryParseInt64(out var ㅁㅊㅇ, true))
                        _매출액 = ㅁㅊㅇ;
                     _법인세비용차감전계속사업이익 = nodes[3].InnerText.HtmlDecode().ParseInt64(true);
                }
            }

            [DataColumn(8)]
            public HtmlNode 순이익_자본금 {
                set {
                    var nodes = value.GetElements();
                    if (nodes[1].InnerText.HtmlDecode().TryParseInt64(out var ㅅㅇㅇ, true))
                        _순이익 = ㅅㅇㅇ;
                    _자본금 = nodes[3].InnerText.HtmlDecode().ParseInt64(true);
                }
            }
        }

        public class 공모정보 : DataRow {
            long _총공모주식수, _액면가, _공모금액;
            string _상장공모;
            LongRange _희망공모가액, _주식수;
            LongRange? _청약한도;
            double? _청약경쟁률;
            long? _확정공모가;
            string[] _주간사;

            [DataColumn(0)]
            public long 총공모주식수 => _총공모주식수;
            [DataColumn(1)]
            public long 액면가 => _액면가;
            [DataColumn(2)]
            public string 상장공모 => _상장공모;
            [DataColumn(3)]
            public LongRange 희망공모가액 => _희망공모가액;
            /// <summary>
            /// 청약경쟁률 N:1
            /// </summary>
            [DataColumn(4)]
            public double? 청약경쟁률 => _청약경쟁률;
            [DataColumn(5)]
            public long? 확정공모가 => _확정공모가;
            /// <summary>
            /// 공모금액 (백만원)
            /// </summary>
            [DataColumn(6)]
            public long 공모금액 => _공모금액;
            [DataColumn(7)]
            public string[] 주간사 => _주간사;
            [DataColumn(8)]
            public LongRange 주식수 => _주식수;
            [DataColumn(9)]
            public LongRange? 청약한도 => _청약한도;

            [DataColumn(0)]
            public HtmlNode 총공모주식수_액면가 {
                set {
                    var nodes = value.GetElements();
                    _총공모주식수 = nodes[1].InnerText.HtmlDecode().ParseInt64(true);
                    _액면가 = nodes[3].InnerText.HtmlDecode().ParseInt64(true);
                }
            }

            [DataColumn(1)]
            public HtmlNode 상장공모_ {
                set {
                    var nodes = value.GetElements();
                    _상장공모 = nodes[1].InnerText.HtmlDecode().Trim();
                }
            }

            [DataColumn(2)]
            public HtmlNode 희망공모가액_청약경쟁률 {
                set {
                    var nodes = value.GetElements();
                    _희망공모가액 = LongRange.Parse(nodes[1].InnerText.HtmlDecode(), '~', true);
                    if (double.TryParse(nodes[3].InnerText.HtmlDecode().Split(':')[0].Trim(), NumberStyles.Any, null, out var ㅊㅇㄱㅈㄹ)) _청약경쟁률 = ㅊㅇㄱㅈㄹ;
                }
            }

            [DataColumn(3)]
            public HtmlNode 확정공모가_공모금액 {
                set {
                    var nodes = value.GetElements();
                    if (nodes[1].InnerText.HtmlDecode().TryParseInt64(out var ㅎㅈㄱㅁㄱ, true)) _확정공모가 = ㅎㅈㄱㅁㄱ;
                    _공모금액 = nodes[3].InnerText.HtmlDecode().ParseInt64(true);
                }
            }

            [DataColumn(4)]
            public HtmlNode 주간사_주식수_청약한도 {
                set {
                    var nodes = value.GetElements();
                    _주간사 = (from ㅅ in nodes[1].InnerText.HtmlDecode().Split(',') select ㅅ.Trim()).ToArray();
                    var ㅈㅊ = nodes[2].InnerText.HtmlDecode().Split('/');
                    if (ㅈㅊ.Length < 2) throw new FormatException($"주식수/청약한도가 없습니다 ({nodes[1].InnerText.HtmlDecode()})");
                    _주식수 = LongRange.Parse(ㅈㅊ[0], '~', true);
                    if (LongRange.TryParse(ㅈㅊ[1], '~', out var ㅊㅇㅎㄷ, true))
                        _청약한도 = ㅊㅇㅎㄷ;
                }
            }
        }

        public class 인수회사 : DataRow {
            readonly List<인수회사Info> infos = new();

            public int Count => infos.Count;

            public bool IsReadOnly => true;

            public void Add(인수회사Info _) =>
                throw new NotSupportedException();

            public void Clear() =>
                throw new NotSupportedException();

            public bool Contains(인수회사Info item) =>
                infos.Contains(item);

            public void CopyTo(인수회사Info[] array, int arrayIndex) =>
                infos.CopyTo(array, arrayIndex);

            public new IEnumerator<인수회사Info> GetEnumerator() =>
                infos.GetEnumerator();

            public bool Remove(인수회사Info _) =>
                throw new NotSupportedException();

            public override void Parse(HtmlNode td) {
                infos.Clear();
                var nodes = td.GetElements();
                foreach (var item in nodes[1..])
                    infos.Add(인수회사Info.Parse(item));
            }

            [DataColumn(0)]
            public 인수회사Info[] 인수회사Infos => infos.ToArray();
        }

        public readonly struct 인수회사Info {
            internal 인수회사Info(string ㅇ, LongRange ㅈ, LongRange ㅊ, string ㄱ) {
                인수회사 = ㅇ;
                주식수 = ㅈ;
                청약한도 = ㅊ;
                기타 = ㄱ;
            }

            public string 인수회사 { get; }
            public LongRange 주식수 { get; }
            /// <summary>
            /// 청약한도 (주)
            /// </summary>
            public LongRange 청약한도 { get; }
            public string 기타 { get; }

            internal static 인수회사Info Parse(HtmlNode node) {
                var nodes = node.GetElements();
                var ㅇ = nodes[0].InnerText.HtmlDecode().Trim();
                var ㅈ = LongRange.Parse(nodes[1].InnerText.HtmlDecode(), '~', true);
                var ㅊ = LongRange.Parse(nodes[2].InnerText.HtmlDecode(), '~', true);
                var ㄱ = nodes[3].InnerText.HtmlDecode().Trim();
                return new 인수회사Info(ㅇ, ㅈ, ㅊ, ㄱ);
            }

            public override string ToString() => $"{인수회사} 주식수: {주식수}, 청약한도: {청약한도}, {기타}";
        }

        /// <summary>
        /// 일부 데이터는 공모정보에 있습니다.
        /// </summary>
        public class 청약일정 : DataRow {
            DateTimeRange _수요예측일, _공모청약일;
            DateTime _배정공고일, _납입일, _환불일;
            DateTime? _상장일, _IR일자, _신규상장일;
            long _총공모주식수, _공모금액;
            long? _확정공모가, _우리사주조합_배정, _기관투자자_배정_한도, _일반청약자_배정_최저, _현재가;
            LongRange _기관투자자_배정, _일반청약자_배정;
            LongRange? _일반청약자_배정_한도;
            double _우리사주조합_증거금율, _일반청약자_증거금율, _의무보유확약;
            double? _기관경쟁률;
            string _IR장소;

            [DataColumn(0)]
            public DateTimeRange 수요예측일 => _수요예측일;
            [DataColumn(1)]
            public DateTimeRange 공모청약일 => _공모청약일;
            [DataColumn(2)]
            public DateTime 배정공고일 => _배정공고일;
            [DataColumn(3)]
            public DateTime 납입일 => _납입일;
            [DataColumn(4)]
            public DateTime 환불일 => _환불일;
            [DataColumn(5)]
            public DateTime? 상장일 => _상장일;
            [DataColumn(6)]
            public long? 확정공모가 => _확정공모가;
            [DataColumn(7)]
            public long 총공모주식수 => _총공모주식수;
            [DataColumn(8)]
            public long 공모금액 => _공모금액;
            [DataColumn(9)]
            public long? 우리사주조합_배정 => _우리사주조합_배정;
            /// <summary>
            /// 우리사주조합 청약증거금율 (100%)
            /// </summary>
            [DataColumn(10)]
            public double 우리사주조합_증거금율 => _우리사주조합_증거금율;
            [DataColumn(11)]
            public LongRange 기관투자자_배정 => _기관투자자_배정;
            [DataColumn(12)]
            public long? 기관투자자_배정_한도 => _기관투자자_배정_한도;
            [DataColumn(13)]
            public LongRange 일반청약자_배정 => _일반청약자_배정;
            [DataColumn(14)]
            public LongRange? 일반청약자_한도 => _일반청약자_배정_한도;
            [DataColumn(15)]
            public double 일반청약자_증거금율 => _일반청약자_증거금율;
            [DataColumn(16)]
            public long? 일반청약자_최저 => _일반청약자_배정_최저;
            [DataColumn(17)]
            public DateTime? IR일자 => _IR일자;
            [DataColumn(18)]
            public string IR장소 => _IR장소;
            [DataColumn(19)]
            public double? 기관경쟁률 => _기관경쟁률;
            [DataColumn(20)]
            public double 의무보유확약 => _의무보유확약;
            [DataColumn(21)]
            public DateTime? 신규상장일 => _신규상장일;
            [DataColumn(22)]
            public long? 현재가 => _현재가;

            [DataColumn(0)]
            public HtmlNode 수요예측일_ {
                set {
                    var nodes = value.GetElements();
                    _수요예측일 = DateTimeRange.Parse(nodes[2].InnerText.HtmlDecode(), "yyyy.MM.dd", "yyyy.MM.dd", '~');
                }
            }

            [DataColumn(1)]
            public HtmlNode 공모청약일_ {
                set {
                    var nodes = value.GetElements();
                    _공모청약일 = DateTimeRange.Parse(nodes[1].InnerText.HtmlDecode(), "yyyy.MM.dd", "yyyy.MM.dd", '~');
                }
            }

            [DataColumn(2)]
            public HtmlNode 배정공고일_ {
                set {
                    var nodes = value.GetElements();
                    _배정공고일 = DateTime.Parse(nodes[1].InnerText.HtmlDecode().Split('(')[0].Trim());
                }
            }

            [DataColumn(3)]
            public HtmlNode 납입일_ {
                set {
                    var nodes = value.GetElements();
                    _납입일 = DateTime.Parse(nodes[1].InnerText.HtmlDecode());
                }
            }

            [DataColumn(4)]
            public HtmlNode 환불일_ {
                set {
                    var nodes = value.GetElements();
                    _환불일 = DateTime.Parse(nodes[1].InnerText.HtmlDecode());
                }
            }

            [DataColumn(5)]
            public HtmlNode 상장일_ {
                set {
                    var nodes = value.GetElements();
                    if (DateTime.TryParse(nodes[1].InnerText.HtmlDecode(), out var v))
                        _상장일 = v;
                }
            }

            [DataColumn(6)]
            public HtmlNode 공모사항_ {
                set {
                    var nodes = value.SelectNodes("./td[2]/table/tr");
                    if (nodes[0].GetElements()[1].InnerText.HtmlDecode().TryParseInt64(out var ㅎㅈㄱㅁㄱ, true))
                        _확정공모가 = ㅎㅈㄱㅁㄱ;

                    {
                        var n = nodes[1].GetElements();
                        _총공모주식수 = n[1].InnerText.HtmlDecode().ParseInt64(true);
                        _공모금액 = n[2].InnerText.HtmlDecode().ParseInt64(true);
                    }

                    {
                        var n = nodes[2].GetElements();
                        if (n[2].InnerText.HtmlDecode().Split('주')[0].TryParseInt64(out var ㅇㄹㅅㅈㅈㅎㅂㅈ, true))
                            _우리사주조합_배정 = ㅇㄹㅅㅈㅈㅎㅂㅈ;
                        _우리사주조합_증거금율 = n[3].InnerText.HtmlDecode().ParseDouble(true);
                    }

                    {
                        var n = nodes[3].GetElements();
                        _기관투자자_배정 = LongRange.Parse(n[1].InnerText.HtmlDecode().Split('주')[0], '~', true);
                        if (n[2].InnerText.HtmlDecode().TryParseInt64(out var ㄱㄱㅌㅈㅈㅂㅈㅎㄷ, true))
                            _기관투자자_배정_한도 = ㄱㄱㅌㅈㅈㅂㅈㅎㄷ;
                    }

                    {
                        var n = nodes[4].GetElements();
                        _일반청약자_배정 = LongRange.Parse(n[1].InnerText.HtmlDecode().Split('주')[0], '~', true);
                        _일반청약자_증거금율 = n[2].InnerText.HtmlDecode().ParseDouble(true);
                    }

                    {
                        var n = nodes[5].GetElements();
                        if (LongRange.TryParse(n[0].InnerText.HtmlDecode().Split('주')[0], '~', out var ㅇㅂㅊㅇㅈㅂㅈㅎㄷ, true))
                            _일반청약자_배정_한도 = ㅇㅂㅊㅇㅈㅂㅈㅎㄷ;
                        if (n[1].InnerText.HtmlDecode().TryParseInt64(out var ㅇㅂㅊㅇㅈㅂㅈㅊㅈ, true))
                            _일반청약자_배정_최저 = ㅇㅂㅊㅇㅈㅂㅈㅊㅈ;
                    }
                }
            }

            [DataColumn(7)]
            public HtmlNode IR일자_IR장소 {
                set {
                    var nodes = value.SelectNodes("./td[2]/table/tr/td");
                    if (DateTime.TryParse(nodes[1].InnerText.HtmlDecode(), out var irㅇㅈ))
                        _IR일자 = irㅇㅈ;
                    _IR장소 = nodes[3].InnerText.HtmlDecode().Trim();
                }
            }

            [DataColumn(8)]
            public HtmlNode 기관경쟁률_의무보유확약 {
                set {
                    var nodes = value.SelectNodes("./td[2]/table/tr/td");
                    if (double.TryParse(nodes[1].InnerText.HtmlDecode().Split(':')[0].Trim(), NumberStyles.Any, null, out var ㄱㄱㄱㅈㄹ)) 
                        _기관경쟁률 = ㄱㄱㄱㅈㄹ;
                    _의무보유확약 = nodes[3].InnerText.HtmlDecode().ParseDouble(true);
                }
            }

            [DataColumn(9, allowThrowException: false)]
            public HtmlNode 신규상장일_현재가 {
                set {
                    var nodes = value.SelectNodes("./td[2]/table/tr/td");
                    if (DateTime.TryParse(nodes[1].InnerText.HtmlDecode(), out var ㅅㄱㅅㅈㅇ))
                        _신규상장일 = ㅅㄱㅅㅈㅇ;
                    if (nodes[3].InnerText.HtmlDecode().Split('원')[0].TryParseInt64(out var ㅎㅈㄱ, true))
                        _현재가 = ㅎㅈㄱ;
                }
            }
        }
    }
}
