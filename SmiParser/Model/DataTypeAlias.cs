using System;
using System.Collections.Generic;
using System.Text;

namespace SmiParser
{
    public class DataTypeAlias : IDataType
    {
        public string Alias { get; set; }
        public IDataType DataType { get; set; }

        public string Name => DataType.Name;

        public SmiEnums.DataTypeBase BaseType => DataType.BaseType;

        public string RestrictionsDescription => Alias +
            ": " + DataType.RestrictionsDescription;

        public bool Validate(string value)
        {
            return DataType.Validate(value);
        }
    }
}
