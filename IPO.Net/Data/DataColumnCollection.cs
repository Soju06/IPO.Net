namespace IPO.Net.Data {
    public class DataColumnCollection<TDataColumn, TDataRow> : IDataColumnCollection<TDataColumn, TDataRow> where TDataColumn : IDataColumn<TDataRow> where TDataRow : IDataRow, new() {
        protected TDataColumn[] columns;
        internal DataColumnCollection(TDataColumn[] columns) {
            this.columns = columns;
        }

        public int Count => columns.Length;

        public bool IsReadOnly => true;

        public TDataColumn this[int index] {
            get => columns[index];
        }

        public TDataColumn? this[string name] {
            get => columns.SingleOrDefault(c => c.Name == name);
        }

        public void Add(TDataColumn item) =>
            throw new NotSupportedException();

        public void Clear() =>
            throw new NotSupportedException();

        public bool Contains(TDataColumn item) =>
            columns.Contains(item);

        public void CopyTo(TDataColumn[] array, int arrayIndex) =>
            columns.CopyTo(array, arrayIndex);

        public IEnumerator<TDataColumn> GetEnumerator() =>
            ((IEnumerable<TDataColumn>)columns).GetEnumerator();

        public bool Remove(TDataColumn item) {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => columns.GetEnumerator();
    }
}
