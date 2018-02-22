using System;
using System.Collections.Generic;
using System.Text;

namespace BerEncoding
{
    public class DecodedObject
    {
        public SmiParser.SmiEnums.DataTypeScope Class { get; set; }
        public bool Constructed { get; set; }

        public SmiParser.SmiEnums.DataTypeBase Type { get; set; }

        public long ValueLength { get; set; }
        public string Value { get; set; }
    }
}
