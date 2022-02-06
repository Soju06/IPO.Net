
namespace IPO.Net.Data {
    public interface IDataColumn<TDataRow> : IEnumerable where TDataRow : IDataRow, new() {
        object? this[int index] { get; }

        string? DisplayName { get; }
        int Index { get; }
        string Name { get; }
        IDataTable<TDataRow> Table { get; }
        Type Type { get; }
    }
}