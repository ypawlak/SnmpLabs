using System;
using System.Collections.Generic;
using System.Text;

namespace BerEncoding
{
    public class DecodedObjectMeta
    {
        public SmiParser.SmiEnums.DataTypeScope Class { get; set; }
        public bool Constructed { get; set; }
        public int Tag { get; set; }

        public long ValueLength { get; set; }

        public byte[] ValueBytes { get; set; }

        public IEnumerable<DecodedObjectMeta> Children { get; set; }
        public DecodedObjectMeta Parent { get; set; }
    }
}
