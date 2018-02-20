using System;
using System.Collections.Generic;
using System.Text;

namespace SmiParser
{
    public class ObjectTypeInfo
    {
        public string Syntax { get; set; }
        public SmiEnums.ObjectTypeAccess Access { get; set; }
        public SmiEnums.ObjectTypeStatus Status { get; set; }
        public string Description { get; set; }
        public OidInfo OidInfo { get; set; }
    }
}
