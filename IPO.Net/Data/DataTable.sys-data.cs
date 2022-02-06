using S = System.Data;

namespace IPO.Net.Data {
    partial class DataTable<TDataRow> {
        public S.DataTable CreateDataTable() {
            var table = new S.DataTable();
            if (typeof(TDataRow).BaseType == typeof(DataRow)) {
                foreach (var column in Columns.OrderBy(col => col.Index)) {
                    if(column is DataColumn<TDataRow> cl && !cl.CanRead) continue;
                    var nb = Nullable.GetUnderlyingType(column.Type);
                    var type = nb ?? column.Type;
                    S.DataColumn col;

                    col = table.Columns.Add(column.Name, type.IsArray ? typeof(string) : type);
                    if (nb != null) col.AllowDBNull = true;
                }

                foreach (var row in this) {
                    var srow = table.NewRow();
                    if (row != null) {
                        foreach (var value in row) {
                            var val = value.Value;
                            var type = val?.GetType();
                            srow[value.Key] = type == null ? DBNull.Value : type.IsArray ? string.Join(",", (object[])val) : val;
                        }
                    }
                    table.Rows.Add(srow);
                }
            } else {
                foreach (var row in this) {
                    var scol = table.NewRow();
                    var srow = table.NewRow();
                    if (row != null) {
                        while (row.Length > table.Columns.Count)
                            table.Columns.Add(table.Columns.Count.ToString());
                        var ci = 0;
                        foreach (var value in row) {
                            var val = value.Value;
                            var type = val?.GetType();
                            scol[ci] = value.Key;
                            srow[ci++] = type == null ? DBNull.Value : type.IsArray ? string.Join(",", ((Array)val).Cast<object>()) : val;
                        }
                    }
                    table.Rows.Add(scol);
                    table.Rows.Add(srow);
                }
            }

            return table;
        }
    }
}
