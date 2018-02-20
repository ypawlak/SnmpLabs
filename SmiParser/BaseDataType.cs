using System;
using System.Collections.Generic;
using System.Text;

namespace SmiParser
{
    public class BaseDataType : IDataType
    {
        public string Name { get ; set ; }
        public SmiEnums.DataTypeBase BaseType { get ; set; }
    }
}
