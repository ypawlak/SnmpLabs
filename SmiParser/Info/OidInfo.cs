using System;
using System.Collections.Generic;
using System.Text;

namespace SmiParser
{
    public class OidInfo
    {
        //public OID Parent { get; set; }
        //public IList<OID> Children { get; set; }
        public string Name { get; set; }
        public long SiblingNo { get; set; }
        public string ParentExpression { get; set; }
    }
}
