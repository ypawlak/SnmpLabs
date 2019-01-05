using System;
using System.Collections.Generic;
using System.Text;
using SmiParser.Info;

namespace SmiParser.Model
{
    public class ObjectNode
    {
        public ObjectNode Parent { get; set; }
        public IList<ObjectNode> Children { get; set; }
        public OidInfo ObjectIdentifier { get; set; }
    }
}
