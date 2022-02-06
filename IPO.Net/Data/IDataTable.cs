
namespace IPO.Net.Data {
    public interface IDataTable<TDataRow> : IList<TDataRow?> where TDataRow : IDataRow, new() {
        IDataColumnCollection<IDataColumn<TDataRow>, TDataRow> Columns { get; }

        IEnumerable Column(int index);
        IEnumerable? Column(string name, bool useDisplayName = false);
        TDataRow ParseAdd(HtmlNode node);
        TDataRow[] ParseAddChildNodes(HtmlNodeCollection node);
        TDataRow[] ParseAddRange(IEnumerable<HtmlNode> nodes);
        TDataRow[] ParseAddRange(params HtmlNode[] nodes);
        TDataRow? Row(int index);
    }
}