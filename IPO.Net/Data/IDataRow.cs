
namespace IPO.Net.Data {
    public interface IDataRow : IEnumerable<KeyValuePair<string, object>> {
        object? this[int index] { get; }

        int Length { get; }

        object? GetValue(string name, bool useDisplayName = false);
        void Parse(HtmlNode td);
    }
}