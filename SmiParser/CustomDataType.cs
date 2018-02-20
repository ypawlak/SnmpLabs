using System;
using System.Collections.Generic;
using System.Text;
using SmiParser.Utils;

namespace SmiParser
{
    public class CustomDataType : BaseDataType
    {
        public enum ClassEnum { IMPLICIT, EXPLICIT };
        
        public long ID { get; set; }
        public SmiEnums.DataTypeScope Scope { get; set; }
        public ClassEnum Class { get; set; }
        public long? Size { get; set; }
        public Range<long> SizeRange { get; set; }
    }
}
