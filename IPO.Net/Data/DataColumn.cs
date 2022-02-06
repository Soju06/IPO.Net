using System.Reflection;

namespace IPO.Net.Data {
    public class DataColumn<TDataRow> : IDataColumn<TDataRow> where TDataRow : IDataRow, new() {
        internal DataColumnAttribute attribute;
        internal PropertyInfo property;

        internal DataColumn(IDataTable<TDataRow> table, PropertyInfo property, DataColumnAttribute attribute) {
            Table = table;
            this.property = property;
            this.attribute = attribute;
        }

        public string Name => property.Name;
        public string? DisplayName => attribute.DisplayName;
        public int Index => attribute.Index;
        public Type Type => property.PropertyType;
        public IDataTable<TDataRow> Table { get; private set; }
        public bool CanRead => property.CanRead;

        public object? this[int index] {
            get => Table[index]?[Index];
        }

        public IEnumerator GetEnumerator() => Table.Column(Index).GetEnumerator();
    }
}
