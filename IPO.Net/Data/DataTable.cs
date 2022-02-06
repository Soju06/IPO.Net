namespace IPO.Net.Data {
     public abstract partial class DataTable<TDataRow> : List<TDataRow?>, IDataTable<TDataRow> where TDataRow : IDataRow, new() {
        public DataTable() {
            Columns = new DataColumnCollection<IDataColumn<TDataRow>, TDataRow>(
                (from pr 
                 in DataRow.TryMakeCache(typeof(TDataRow)) 
                 select new DataColumn<TDataRow>(this, pr.Item1, pr.Item2))
                    .ToArray());
        }

        public TDataRow ParseAdd(HtmlNode node) {
            TDataRow row = new();
            row.Parse(node);
            Add(row);
            return row;
        }

        public IDataColumnCollection<IDataColumn<TDataRow>, TDataRow> Columns { get; private set; }

        public IEnumerable Column(int index) =>
            from row in this select row[index];

        public IEnumerable? Column(string name, bool useDisplayName = false) {
            var di = DataRow.TryFindValue(typeof(TDataRow), name, useDisplayName);
            if (!di.HasValue) return null;
            var (prop, _) = di.Value;
            return from row in this select prop.GetValue(row);
        }

        public TDataRow? Row(int index) {
            if (index < 0 || index >= Count) return default;
            return this[index];
        }

        public TDataRow[] ParseAddRange(params HtmlNode[] nodes) {
            var nLen = nodes.Length;
            var drows = new TDataRow[nLen];
            for (int i = 0; i < nLen; i++)
                drows[i] = ParseAdd(nodes[i]);
            return drows;
        }

        public TDataRow[] ParseAddRange(IEnumerable<HtmlNode> nodes) {
            List<TDataRow> drows = new();
            foreach (var node in nodes)
                drows.Add(ParseAdd(node));
            return drows.ToArray();
        }

        public TDataRow[] ParseAddChildNodes(HtmlNodeCollection node) =>
            ParseAddRange(node.Where(n => n.NodeType == HtmlNodeType.Element));
    }
}
