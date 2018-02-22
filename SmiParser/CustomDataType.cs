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
        public override string RestrictionsDescription => base.RestrictionsDescription
            + ": " + SmiEnums.GetString(BaseType)
            + Environment.NewLine
            + GetSizeRestrictionsDescription();

        private string GetSizeRestrictionsDescription()
        {
            return Size != null
                ? string.Format("Size: {0}", Size)
                : (SizeRange != null
                    ? string.Format("Size: {0}..{1}", SizeRange.Minimum, SizeRange.Maximum)
                    : "No size restrictions");
        }

        public override bool Validate(string value)
        {
            return base.Validate(value) && ValidateSize(value);
        }

        private bool ValidateSize(string value)
        {
            if(Size == null)
            {
                if (SizeRange == null)
                    return true;
                else
                {
                    switch(BaseType)
                    {
                        case SmiEnums.DataTypeBase.INTEGER:
                            return SizeRange.ContainsValue(long.Parse(value));
                        case SmiEnums.DataTypeBase.OCTET_STRING:
                            return SizeRange.ContainsValue(value.Length);
                    }
                }
            }
            else
            {
                switch (BaseType)
                {
                    case SmiEnums.DataTypeBase.INTEGER:
                        return long.Parse(value) < Size;
                    case SmiEnums.DataTypeBase.OCTET_STRING:
                        return value.Length <= Size;
                }
            }

            return false;
        }
    }
}
