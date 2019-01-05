using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SmiParser.Model
{
    public class SmiEnums
    {
        private const string ContextSpecificScopeString = "CONTEXT-SPECIFIC";

        public enum DataTypeScope
        {
            UNIVERSAL,
            APPLICATION,
            CONTEXT_SPECIFIC,
            PRIVATE
        };

        private const string OctetString = "OCTET STRING";
        private const string ObjectIdentifier = "OBJECT IDENTIFIER";
        private const string BitString = "BIT STRING";
        public enum DataTypeBase
        {
            INTEGER,
            OCTET_STRING,
            OBJECT_IDENTIFIER,
            NULL,
            SEQUENCE,
            BIT_STRING
        }

        public enum ObjectTypeAccess
        {
            read_only,
            read_write,
            write_only,
            not_accessible
        }

        public enum ObjectTypeStatus
        {
            mandatory,
            optional,
            obsolete
        }

        public static T Parse<T>(string strVal)
            where T : struct
        {
            if (Enum.TryParse(strVal, out T result))
                return result;
            if (typeof(T) == typeof(DataTypeScope)
                && strVal.Equals(ContextSpecificScopeString, StringComparison.OrdinalIgnoreCase))
                return (T)(object)DataTypeScope.CONTEXT_SPECIFIC;
            if(typeof(T) == typeof(DataTypeBase))
            {
                if (strVal.Equals(OctetString, StringComparison.OrdinalIgnoreCase))
                    return (T)(object)DataTypeBase.OCTET_STRING;
                if (strVal.Equals(ObjectIdentifier, StringComparison.OrdinalIgnoreCase))
                    return (T)(object)DataTypeBase.OBJECT_IDENTIFIER;
                if (strVal.Equals(BitString, StringComparison.OrdinalIgnoreCase))
                    return (T)(object)DataTypeBase.BIT_STRING;
            }
            if (typeof(T) == typeof(ObjectTypeAccess))
                return (T)Enum.Parse(typeof(ObjectTypeAccess), strVal.Replace("-", "_"));

            throw new InvalidOperationException();
        }

        public static string GetString<T>(T sEnum)
        {
            if(typeof(T) == typeof(DataTypeScope) && (DataTypeScope)(object)sEnum == DataTypeScope.CONTEXT_SPECIFIC)
                return ContextSpecificScopeString;

            if(typeof(T) == typeof(DataTypeBase))
            {
                switch ((DataTypeBase)(object)sEnum)
                {
                    case DataTypeBase.OBJECT_IDENTIFIER: return ObjectIdentifier;
                    case DataTypeBase.OCTET_STRING: return OctetString;
                    case DataTypeBase.BIT_STRING: return BitString;
                    default: break;
                }
            }

            if (typeof(T) == typeof(ObjectTypeAccess))
                return sEnum.ToString().Replace("_", "-");

            return sEnum.ToString();
        }

        public static IEnumerable<string> GetEnumCustomizedTextList<T>()
        {
            return Enum.GetValues(typeof(T))
                        .Cast<T>()
                        .Select(x => GetString(x));
        }
    }
}
