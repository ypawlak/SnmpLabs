using SmiParser.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmiParser
{
    public class ObjectType
    {
        public string Syntax { get; set; }
        public IDataType DataType { get; set; }
        public SmiEnums.ObjectTypeAccess Access { get; set; }
        public SmiEnums.ObjectTypeStatus Status { get; set; }
        public string Description { get; set; }
        public long? Size { get; set; }
        public Range<long> SizeRange { get; set; }

        public string DisplayName
        {
            get
            {
                return DataType != null
                    ? string.Format("[{0} ({1})]", DataType.Name, SmiEnums.GetString(DataType.BaseType))
                    : string.Empty;
            }
        }
    }
}
