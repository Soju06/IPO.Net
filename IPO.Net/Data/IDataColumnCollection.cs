namespace IPO.Net.Data {
    public interface IDataColumnCollection<TDataColumn, TDataRow> : ICollection<TDataColumn> where TDataColumn : IDataColumn<TDataRow> where TDataRow : IDataRow, new() {
        TDataColumn this[int index] { get; }
        TDataColumn? this[string name] { get; }
    }
}