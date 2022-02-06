using System;
using System.Collections.Generic;
using System.Text;

namespace IPO.Net.Data {
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DataColumnAttribute : Attribute {
        public DataColumnAttribute(int index, string? displayName = null, string? customConvertFuncName = null, string? convertParameter = null, bool allowThrowException = true) {
            Index = index;
            DisplayName = displayName;
            AllowThrowException = allowThrowException;
            CustomConvertFuncName = customConvertFuncName;
            ConvertParameter = convertParameter;
        }

        public int Index { get; }

        public string? DisplayName { get; }

        public string? CustomConvertFuncName { get; }

        public string? ConvertParameter { get; }

        public bool AllowThrowException { get; }
    }
}
