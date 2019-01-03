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

        #region Print
        public string DisplayName
        {
            get
            {
                return DataType != null
                    ? string.Format("[{0} ({1})]", DataType.Name, SmiEnums.GetString(DataType.BaseType))
                    : string.Empty;
            }
        }

        public string RestrictionsDescrption =>
            GetSizeRestrictionsDescription() +
            GetSyntaxRestrictionsDesc() + Environment.NewLine;

        private string GetSyntaxRestrictionsDesc()
        {
            return "Syntax: " + DataType == null
                ? Syntax
                : DataType.RestrictionsDescription;
        }

        private string GetSizeRestrictionsDescription()
        {
            return Size != null
                ? string.Format("Size: {0}", Size)
                : (SizeRange != null
                    ? string.Format("Size: {0}..{1}", SizeRange.Minimum, SizeRange.Maximum)
                    : string.Empty);
        }
        #endregion

        public bool Validate(string value)
        {
            if (DataType == null)
                return false;
            return DataType.Validate(value) && ValidateSize(value);

        }

        private bool ValidateSize(string value)
        {
            if (Size == null)
            {
                if (SizeRange == null)
                    return true;
                else
                {
                    switch (DataType.BaseType)
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
                switch (DataType.BaseType)
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
