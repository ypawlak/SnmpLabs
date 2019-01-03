using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SmiParser.Info
{
    public class AliasInfoValueEqualityComparer : IEqualityComparer<AliasInfo>
    {
        public bool Equals(AliasInfo x, AliasInfo y)
        { 
            return x.Alias.Equals(y.Alias) &&
                x.OriginalBaseTypeName.Equals(y.OriginalBaseTypeName) &&
                x.OriginalComplexTypeName.Equals(y.OriginalComplexTypeName);
        }

        public int GetHashCode(AliasInfo obj)
        {
            int hash = 17;
            hash = hash * 23 + (obj.Alias ?? "").GetHashCode();
            hash = hash * 23 + (obj.OriginalBaseTypeName ?? "").GetHashCode();
            hash = hash * 23 + (obj.OriginalComplexTypeName ?? "").GetHashCode();
            return hash;
        }
    }
}