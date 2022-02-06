using System.Web;

namespace IPO.Net {
    /// <summary>
    /// 38.co.kr
    /// </summary>
    public partial class Ipo : IDisposable {
        HttpClient http = new();
        Encoding encoding = Encoding.GetEncoding("euc-kr");
        bool disposedValue;

        async Task<int> GetPageCountAsync(Func<int, string> makeUri, CancellationToken token = default) {
            int lastPage = -2, curPage = 1, nwPage = -1;
            L_REF:
            using (var res = await http.GetAsync(makeUri.Invoke(curPage), token)) {
                res.EnsureSuccessStatusCode();
                var doc = await res.Content.HtmlAsync(encoding);
                try {
                    if (nwPage == -1 && !int.TryParse(doc.DocumentNode.SelectSingleNode("//div[@align='center']/font[@color='green']/b")?.InnerText.Trim(), out nwPage))
                        nwPage = -1;
                    lastPage = GetLastPage(doc);
                } catch {

                }
            }

            if (lastPage == -1) return nwPage;
            else if (lastPage == -2) return -1;

            if (lastPage > curPage) {
                nwPage = curPage = lastPage;
                goto L_REF;
            } else throw new InvalidOperationException("마지막 페이지가 현재 페이지보다 적을 수 없습니다.");
        }

        async Task<TDataTable> GetDataTableAsync<TDataRow, TDataTable>(DateTimeRange range, Func<int, string> makeUri, Func<TDataRow, DateTime> getDateTime, Func<HtmlDocument, HtmlNodeCollection> getNode, CancellationToken token = default) where TDataRow : IDataRow, new() where TDataTable : IDataTable<TDataRow>, new() {
            var lastPage = await GetPageCountAsync(makeUri, token);
            var table = new TDataTable();

            if (lastPage > 0) {
                bool __br = false;
                for (int i = 0; i < lastPage; i++) {
                    using var r = await http.GetAsync(makeUri.Invoke(i + 1), token);
                    var nodes = table.ParseAddChildNodes(getNode.Invoke(await r.Content.HtmlAsync(encoding)));
                    var nLen = nodes.Length;
                    for (int ni = 0; ni >= 0 && ni < nLen; ni++) {
                        var dt = getDateTime.Invoke(nodes[ni]);
                        if (!range.IsInner(dt)) {
                            table.RemoveAt(ni--);
                            nLen--;
                            if (dt < range.StartAt) __br = true;
                        }
                    }

                    if (__br) break;
                }
            }
            return table;
        }

        async Task<TDataTable> GetDataTableAsync<TDataRow, TDataTable>(PageRange pages, Func<int, string> makeUri, Func<HtmlDocument, HtmlNodeCollection> getNode, CancellationToken token = default) where TDataRow : IDataRow, new() where TDataTable : IDataTable<TDataRow>, new() {
            var lastPage = await GetPageCountAsync(makeUri, token);
            var table = new TDataTable();

            if (pages.Offset + 1 > lastPage) return table;
            pages = new(pages.Offset + 1, (pages.IsInfinite ? lastPage - pages.Offset : Math.Min(lastPage - pages.Offset, (int)pages.Count)));

            if (lastPage > 0) {
                foreach (var page in pages) {
                    using var r = await http.GetAsync(makeUri.Invoke(page), token);
                    table.ParseAddChildNodes(getNode.Invoke(await r.Content.HtmlAsync(encoding)));
                }
            }
            return table;
        }

        async Task<TDataTable> GetDataTableAsync<TDataRow, TDataTable>(Uri uri, Func<HtmlDocument, HtmlNodeCollection> func, CancellationToken token = default) where TDataRow : IDataRow, new() where TDataTable : IDataTable<TDataRow>, new() {
            using var res = await http.GetAsync(uri, token);
            return await MakeDataSet<TDataRow, TDataTable>(res, encoding, func);
        }

        static async Task<TDataTable> MakeDataSet<TDataRow, TDataTable>(HttpResponseMessage response, Encoding encoding, Func<HtmlDocument, HtmlNodeCollection> func)
            where TDataRow : IDataRow, new() where TDataTable : IDataTable<TDataRow>, new() {
            response.EnsureSuccessStatusCode();
            var set = new TDataTable();
            set.ParseAddChildNodes(func.Invoke(await response.Content.HtmlAsync(encoding)));
            return set;
        }

        static int GetLastPage(HtmlDocument document) {
            var nd = document.DocumentNode.SelectSingleNode("//a[@href and contains(text(),'[마지막]')]");
            if (nd == null) {
                nd = document.DocumentNode.SelectSingleNode("//a[@href and contains(text(),'[다음]')]");
                if (nd != null) {
                    foreach (var elem in nd.ParentNode.ChildNodes.Where(elem => elem.NodeType == HtmlNodeType.Element && elem.Name == "a")) {
                        if (!elem.InnerText.Contains("다음") && elem.GetAttributeValue("href", null) != null) nd = elem;
                        else break;
                    }

                    if (nd == null) goto THROW_NOT_FOUND;
                } else return -1;
            }

            var href = nd.GetAttributeValue("href", null)?.HtmlDecode();
            if(href == null) goto THROW_NOT_FOUND;

            return int.Parse(HttpUtility.ParseQueryString(new Uri(new Uri("https://www.38.co.kr/"), href).Query).Get("page"));

            THROW_NOT_FOUND:
            throw new FormatException("마지막 페이지를 찾을 수 없습니다.");
        }

        static HtmlNodeCollection GetXPaths(HtmlDocument document, string xpath) {
            var nd = document.DocumentNode.SelectNodes(xpath);
            if (nd == null || nd.Count < 1) throw new FormatException($"올바르지 않은 노드입니다. ({xpath})");
            return nd;
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    http.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
