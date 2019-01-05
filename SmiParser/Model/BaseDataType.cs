using System;
using System.Collections.Generic;
using System.Text;

namespace SmiParser.Model
{
    public class BaseDataType : IDataType
    {
        public string Name { get ; set ; }
        public SmiEnums.DataTypeBase BaseType { get ; set; }

        public virtual string RestrictionsDescription => Name;

        public virtual bool Validate(string value)
        {
            switch(BaseType)
            {
                case SmiEnums.DataTypeBase.INTEGER:
                    return long.TryParse(value, out var outVal);
                case SmiEnums.DataTypeBase.OCTET_STRING:
                    return true;
                default:
                    return false;
            }
        }
    }
}
