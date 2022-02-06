namespace IPO.Net {
    public partial class Ipo {
        async Task<TDataTable> GetIPOTableAsync<TDataRow, TDataTable>(Uri uri, string xpath, CancellationToken token = default) 
            where TDataRow : IDataRow, new() where TDataTable : IDataTable<TDataRow>, new() {
            return await GetDataTableAsync<TDataRow, TDataTable>(uri, doc => GetXPaths(doc, xpath), token);
        }

        async Task<TDataTable> GetIPOTableAsync<TDataRow, TDataTable>(PageRange pages, Func<int, string> makeUri, string xpath, CancellationToken token = default)
            where TDataRow : IDataRow, new() where TDataTable : IDataTable<TDataRow>, new() {
            return await GetDataTableAsync<TDataRow, TDataTable>(pages, makeUri,
                doc => GetXPaths(doc, xpath), token);
        }

        async Task<TDataTable> GetIPOTableAsync<TDataRow, TDataTable>(DateTimeRange range, Func<int, string> makeUri, Func<TDataRow, DateTime> getDateTime, string xpath, CancellationToken token = default)
            where TDataRow : IDataRow, new() where TDataTable : IDataTable<TDataRow>, new() {
            return await GetDataTableAsync<TDataRow, TDataTable>(range,
                makeUri, getDateTime, doc => GetXPaths(doc, xpath), token);
        }


        /// <summary>
        /// 청구종목
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<ClaimStocks> GetClaimStocksAsync(int page, CancellationToken token = default) {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            return await GetIPOTableAsync<ClaimStock, ClaimStocks>(new($"https://www.38.co.kr/html/ipo/ipo.htm?key=2&page={page}"),
                "//table[@summary='청구종목']/tbody/tr", token);
        }

        /// <summary>
        /// 청구종목
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<ClaimStocks> GetClaimStocksAsync(PageRange pages, CancellationToken token = default) {
            if (pages.Offset < 0) throw new ArgumentOutOfRangeException(nameof(pages));
            return await GetIPOTableAsync<ClaimStock, ClaimStocks>(pages,
                page => $"https://www.38.co.kr/html/ipo/ipo.htm?key=2&page={page}",
                "//table[@summary='청구종목']/tbody/tr", token);
        }

        /// <summary>
        /// 청구종목
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<ClaimStocks> GetClaimStocksAsync(DateTimeRange range, CancellationToken token = default) {
            return await GetIPOTableAsync<ClaimStock, ClaimStocks>(range,
                page => $"https://www.38.co.kr/html/ipo/ipo.htm?key=2&page={page}",
                row => row.청구일, "//table[@summary='청구종목']/tbody/tr", token);
        }

        /// <summary>
        /// 승인종목
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<ApprovalStocks> GetApprovalStocksAsync(int page, CancellationToken token = default) {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            return await GetIPOTableAsync<ApprovalStock, ApprovalStocks>(new($"https://www.38.co.kr/html/ipo/ipo.htm?key=1&page={page}"),
                "//table[@summary='승인종목']/tbody/tr", token);
        }

        /// <summary>
        /// 승인종목
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<ApprovalStocks> GetApprovalStocksAsync(PageRange pages, CancellationToken token = default) {
            if (pages.Offset < 0) throw new ArgumentOutOfRangeException(nameof(pages));
            return await GetIPOTableAsync<ApprovalStock, ApprovalStocks>(pages,
                page => $"https://www.38.co.kr/html/ipo/ipo.htm?key=1&page={page}",
                "//table[@summary='승인종목']/tbody/tr", token);
        }

        /// <summary>
        /// 승인종목
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<ApprovalStocks> GetApprovalStocksAsync(DateTimeRange range, CancellationToken token = default) {
            return await GetIPOTableAsync<ApprovalStock, ApprovalStocks>(range,
                page => $"https://www.38.co.kr/html/ipo/ipo.htm?key=1&page={page}",
                row => row.승인일, 
                "//table[@summary='승인종목']/tbody/tr", token);
        }

        /// <summary>
        /// 수요예측일정
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<DemandForecastSchedules> GetDemandForecastSchedulesAsync(int page, CancellationToken token = default) {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            return await GetIPOTableAsync<DemandForecastSchedule, DemandForecastSchedules>(new($"http://www.38.co.kr/html/fund/index.htm?o=r&page={page}"),
                "//table[@summary='수요예측일정']/tbody/tr[@bgcolor]", token);
        }

        /// <summary>
        /// 수요예측일정
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<DemandForecastSchedules> GetDemandForecastSchedulesAsync(PageRange pages, CancellationToken token = default) {
            if (pages.Offset < 0) throw new ArgumentOutOfRangeException(nameof(pages));
            return await GetIPOTableAsync<DemandForecastSchedule, DemandForecastSchedules>(pages,
                page => $"http://www.38.co.kr/html/fund/index.htm?o=r&page={page}",
                "//table[@summary='수요예측일정']/tbody/tr[@bgcolor]", token);
        }

        /// <summary>
        /// 수요예측일정
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <param name="startCriteria">true: 시작일 기준 false: 종료일 기준</param>
        public async Task<DemandForecastSchedules> GetDemandForecastSchedulesAsync(DateTimeRange range, bool startCriteria = true, CancellationToken token = default) {
            return await GetIPOTableAsync<DemandForecastSchedule, DemandForecastSchedules>(range,
                page => $"http://www.38.co.kr/html/fund/index.htm?o=r&page={page}",
                row => row.수요예측일 == null ? DateTime.MinValue : startCriteria ? row.수요예측일.Value.StartAt : row.수요예측일.Value.EndAt,
                "//table[@summary='수요예측일정']/tbody/tr[@bgcolor]", token);
        }

        /// <summary>
        /// 수요예측결과
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<DemandForecastResults> GetDemandForecastResultsAsync(int page, CancellationToken token = default) {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            return await GetIPOTableAsync<DemandForecastResult, DemandForecastResults>(new($"http://www.38.co.kr/html/fund/index.htm?o=r1&page={page}"),
                "//table[@summary='수요예측결과']/tbody/tr", token);
        }

        /// <summary>
        /// 수요예측결과
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<DemandForecastResults> GetDemandForecastResultsAsync(PageRange pages, CancellationToken token = default) {
            if (pages.Offset < 0) throw new ArgumentOutOfRangeException(nameof(pages));
            return await GetIPOTableAsync<DemandForecastResult, DemandForecastResults>(pages,
                page => $"http://www.38.co.kr/html/fund/index.htm?o=r1&page={page}",
                "//table[@summary='수요예측결과']/tbody/tr", token);
        }

        /// <summary>
        /// 수요예측결과
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<DemandForecastResults> GetDemandForecastResultsAsync(DateTimeRange range, CancellationToken token = default) {
            return await GetIPOTableAsync<DemandForecastResult, DemandForecastResults>(range,
                page => $"http://www.38.co.kr/html/fund/index.htm?o=r1&page={page}",
                row => row.예측일 ?? DateTime.MinValue,
                "//table[@summary='수요예측결과']/tbody/tr", token);
        }

        /// <summary>
        /// 공모청약일정
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<IPOSubscriptionSchedules> GetIPOSubscriptionSchedulesAsync(int page, CancellationToken token = default) {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            return await GetIPOTableAsync<IPOSubscriptionSchedule, IPOSubscriptionSchedules>(new($"http://www.38.co.kr/html/fund/index.htm?o=k&page={page}"),
                "//table[@summary='공모주 청약일정']/tbody/tr", token);
        }

        /// <summary>
        /// 공모청약일정
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<IPOSubscriptionSchedules> GetIPOSubscriptionSchedulesAsync(PageRange pages, CancellationToken token = default) {
            if (pages.Offset < 0) throw new ArgumentOutOfRangeException(nameof(pages));
            return await GetIPOTableAsync<IPOSubscriptionSchedule, IPOSubscriptionSchedules>(pages,
                page => $"http://www.38.co.kr/html/fund/index.htm?o=k&page={page}",
                "//table[@summary='공모주 청약일정']/tbody/tr", token);
        }

        /// <summary>
        /// 공모청약일정
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <param name="startCriteria">true: 시작일 기준 false: 종료일 기준</param>
        public async Task<IPOSubscriptionSchedules> GetIPOSubscriptionSchedulesAsync(DateTimeRange range, bool startCriteria = true, CancellationToken token = default) {
            return await GetIPOTableAsync<IPOSubscriptionSchedule, IPOSubscriptionSchedules>(range,
                page => $"http://www.38.co.kr/html/fund/index.htm?o=k&page={page}",
                row => startCriteria ? row.공모주일정.StartAt : row.공모주일정.EndAt,
                "//table[@summary='공모주 청약일정']/tbody/tr", token);
        }

        /// <summary>
        /// 신규상장
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<NewlyListedStocks> GetNewlyListedStocksAsync(int page, CancellationToken token = default) {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            return await GetIPOTableAsync<NewlyListedStock, NewlyListedStocks>(new($"http://www.38.co.kr/html/fund/index.htm?o=nw&page={page}"),
                "//table[@summary='신규상장종목']/tbody/tr", token);
        }

        /// <summary>
        /// 신규상장
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<NewlyListedStocks> GetNewlyListedStocksAsync(PageRange pages, CancellationToken token = default) {
            if (pages.Offset < 0) throw new ArgumentOutOfRangeException(nameof(pages));
            return await GetIPOTableAsync<NewlyListedStock, NewlyListedStocks>(pages,
                page => $"http://www.38.co.kr/html/fund/index.htm?o=nw&page={page}",
                "//table[@summary='신규상장종목']/tbody/tr", token);
        }

        /// <summary>
        /// 신규상장
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<NewlyListedStocks> GetNewlyListedStocksAsync(DateTimeRange range, CancellationToken token = default) {
            return await GetIPOTableAsync<NewlyListedStock, NewlyListedStocks>(range,
                page => $"http://www.38.co.kr/html/fund/index.htm?o=nw&page={page}",
                row => row.신규상장일,
                "//table[@summary='신규상장종목']/tbody/tr", token);
        }

        /// <summary>
        /// 기업분석
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public async Task<CorporateAnalysis> GetCorporateAnalysisAsync(string id, CancellationToken token = default) {
            using (var res = await new HttpClient().GetAsync($"http://www.38.co.kr/html/fund/index.htm?o=v&no={id}", token)) {
                res.EnsureSuccessStatusCode();
                CorporateAnalysis analysis = new();
                analysis.Parse(await res.Content.HtmlAsync(encoding));
                return analysis;
            }
        }
    }
}
