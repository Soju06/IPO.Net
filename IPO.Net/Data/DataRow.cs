using System.Globalization;
using System.Reflection;

namespace IPO.Net.Data {
    public class DataRow : IDataRow {
        internal readonly static Dictionary<Type, Func<HtmlNode, string?, (bool isSuccess, object? value)>> convertFuncs = new() {
            {
                typeof(byte),
                (node, _) => new(byte.TryParse(node.InnerText.HtmlDecode().Trim(), NumberStyles.Any, null, out var value), value)
            },
            {
                typeof(sbyte),
                (node, _) => new(sbyte.TryParse(node.InnerText.HtmlDecode().Trim(), NumberStyles.Any, null, out var value), value)
            },
            {
                typeof(int),
                (node, _) => new(int.TryParse(node.InnerText.HtmlDecode().Trim(), NumberStyles.Any, null, out var value), value)
            },
            {
                typeof(uint),
                (node, _) => new(uint.TryParse(node.InnerText.HtmlDecode().Trim(), NumberStyles.Any, null, out var value), value)
            },
            {
                typeof(long),
                (node, _) => new(long.TryParse(node.InnerText.HtmlDecode().Trim(), NumberStyles.Any, null, out var value), value)
            },
            {
                typeof(ulong),
                (node, _) => new(ulong.TryParse(node.InnerText.HtmlDecode().Trim(), NumberStyles.Any, null, out var value), value)
            },
            {
                typeof(float),
                (node, _) => new(float.TryParse(node.InnerText.HtmlDecode().Trim(), NumberStyles.Any, null, out var value), value)
            },
            {
                typeof(double),
                (node, _) => new(double.TryParse(node.InnerText.HtmlDecode().Trim(), NumberStyles.Any, null, out var value), value)
            },
            {
                typeof(DateTime),
                (node, _) => new(DateTime.TryParse(node.InnerText.HtmlDecode().Trim(), out var value), value)
            },
            {
                typeof(TimeSpan),
                (node, _) => new(TimeSpan.TryParse(node.InnerText.HtmlDecode().Trim(), out var value), value)
            },
            {
                typeof(string),
                (node, _) => new(true, node.InnerText.HtmlDecode().Trim())
            },
            {
                typeof(DateTimeRange),
                (node, _) => new(DateTimeRange.TryParse(node.InnerText.HtmlDecode().Trim(),
                    "yyyy.MM.dd", "M.d", '~', CultureInfo.GetCultureInfo("ko-kr"), 1, out var value), value)
            },
            {
                typeof(LongRange),
                (node, p) => new(LongRange.TryParse(node.InnerText.HtmlDecode().Trim(), '~', out var value, p?.Length >= 1 && p[0] == 'a'), value)
            },
            {
                typeof(HtmlNode),
                (node, _) => new(true, node)
            },
            {
                typeof(string[]),
                (node, par) => {
                    try {
                        var spc = ',';
                        var usetrim = true;
                        if (par?.Length > 0) spc = par[0];
                        if (par?.Length > 1) usetrim = par[1] == 'Y';
                        var ss = node.InnerText.HtmlDecode().Trim().Split(spc);
                        if (usetrim) {
                            var sLen = ss.Length;
                            for (int i = 0; i < sLen; i++)
                                ss[i] = ss[i].Trim();
                        }
                        return new(true, ss);
                    } catch {
                        return new(false, null);
                    }
                }
            }
        };

        internal readonly static Dictionary<string, Func<HtmlNode, string?, (bool isSuccess, object? value)>> customConvertFuncs = new() {
            { 
                "date_mm_dd",
                (node, par) => new(DateTime.TryParseExact(node.InnerText.HtmlDecode().Trim(), "M/d", CultureInfo.GetCultureInfo("ko-kr"),
                    DateTimeStyles.AllowWhiteSpaces, out var value), value)
            },
            {
                "경쟁률",
                (node, par) => new(double.TryParse(node.InnerText.HtmlDecode().Trim().Split(':')[0], out var value), value)
            },
            {
                "100%",
                (node, par) => new(double.TryParse(node.InnerText.HtmlDecode().Trim().Split('%')[0], out var value), value)
            },
            {
                "long_dOnly",
                (node, par) => new(long.TryParse(node.InnerText.HtmlDecode().Trim().Where(c => char.IsDigit(c)).ToArray(), out var value), value)
            },
            {
                "empty-",
                (node, par) => {
                    var str = node.InnerText.HtmlDecode().Trim();
                    return new(true, str == "-" ? "" : str);
                }
            }
        };

        internal readonly static Dictionary<Type, (PropertyInfo, DataColumnAttribute)[]> cachedProperty = new();

        internal static (PropertyInfo, DataColumnAttribute)[] TryMakeCache(Type type) {
            if (cachedProperty.TryGetValue(type, out var props)) return props;
            List<(PropertyInfo, DataColumnAttribute)> ls = new();
            foreach (var prop in type.GetProperties()) {
                var att = prop.GetCustomAttribute<DataColumnAttribute>();
                if (att != null) ls.Add(new(prop, att));
            }
            props = ls.ToArray();
            cachedProperty.Add(type, props);
            return props;
        }

        internal static (PropertyInfo, DataColumnAttribute)? TryFindValue(Type type, string name, bool useDisplayName = false) =>
            TryMakeCache(type).SingleOrDefault(
                p => useDisplayName ? (p.Item2.DisplayName ?? p.Item1.Name) == name : p.Item1.Name == name);

        public object? this[int index] {
            get {
                var prs = TryMakeCache(GetType());
                if (index < 0 || index >= prs.Length) throw new ArgumentOutOfRangeException(nameof(index));
                return prs[index].Item1.GetValue(this);
            }
        }

        public int Length => ReadableLength;

        public int ReadableLength => TryMakeCache(GetType()).Count(pr => pr.Item1.CanRead);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            foreach (var pr in TryMakeCache(GetType())) {
                var (prop, att) = pr;
                if (!prop.CanRead) continue;
                yield return new(att.DisplayName ?? prop.Name, prop.GetValue(this));
            }
        }

        public object? GetValue(string name, bool useDisplayName = false) {
            var n = TryFindValue(GetType(), name, useDisplayName);
            if (n.HasValue) {
                var (prop, _) = n.Value;
                if (!prop.CanRead) throw new MemberAccessException($"{prop.Name}을 액세스할 수 없습니다.");
                return prop.GetValue(this);
            }
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual void Parse(HtmlNode td) {
            var nodes = td.GetElements();
            var nLen = nodes.Length;
            var props = TryMakeCache(GetType());
            foreach (var pr in props) {
                var (prop, att) = pr;
                if (!prop.CanWrite) continue;
                try {
                    var index = att.Index;

                    if (index < 0 || index >= nLen) {
                        if (att.AllowThrowException) throw new IndexOutOfRangeException($"{prop.Name}의 인덱스가 데이터 길이보다 큽니다.");
                        continue;
                    }

                    var elem = nodes[index];
                    var nnType = Nullable.GetUnderlyingType(prop.PropertyType);
                    var type = nnType ?? prop.PropertyType;
                    var isNullable = nnType != null;

                    Func<HtmlNode, string?, (bool isSuccess, object? value)>? func = null;

                    if (att.CustomConvertFuncName != null) {
                        if (!customConvertFuncs.TryGetValue(att.CustomConvertFuncName, out func)) {
                            if (att.AllowThrowException) throw new InvalidCastException($"{prop.Name}의 {type.Name}타입 캐스트 {att.CustomConvertFuncName}이가 등록되있지 않습니다.");
                            continue;
                        }
                    }

                    if (func == null && !convertFuncs.TryGetValue(type, out func)) {
                        if (att.AllowThrowException) throw new InvalidCastException($"{prop.Name}의 {type.Name}타입 캐스트가 등록되있지 않습니다.");
                        continue;
                    }

                    var (isSuccess, value) = func.Invoke(elem, att.ConvertParameter);

                    if (!isSuccess) {
                        if (!isNullable && att.AllowThrowException) throw new InvalidCastException($"{prop.Name}의 {type.Name}타입 캐스트를 실패했습니다. ({elem.InnerText})");
                        continue;
                    }

                    prop.SetValue(this, value);
                } catch (Exception ex) {
                    if (att.AllowThrowException) throw new InvalidCastException("캐스트를 실패했습니다.", ex);
                }
            }
        }
    }
}
