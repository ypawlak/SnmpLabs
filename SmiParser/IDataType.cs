using System;
using System.Collections.Generic;
using System.Text;
using static SmiParser.SmiEnums;

namespace SmiParser
{
    public interface IDataType
    {
        string Name { get; }
        DataTypeBase BaseType { get; }
    }
}
