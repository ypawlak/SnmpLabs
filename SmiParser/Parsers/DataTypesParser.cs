using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using SmiParser.Utils;
using SmiParser.Model;

namespace SmiParser.Parsers
{
    public static class DataTypesParser
    {
        const string DataTypeNameGrp = "dtName";
        const string DataTypeScopeGrp = "dtScope";
        const string DataTypeIdGrp = "dtId";
        const string DataTypeClassGrp = "dtClass";
        const string DataTypeBaseTypeGrp = "dtBaseType";
        const string DataTypeSizeGrp = "dtSize";
        const string DataTypeSizeMinGrp = "dtSizeMin";
        const string DataTypeSizeMaxGrp = "dtSizeMax";

        public static IEnumerable<CustomDataType> ParseAllDataTypes(string mibFile)
        {
            Regex rx = GetDataTypesRegex(new List<string>());
            return ParseDataTypesRegexResults(rx.Matches(mibFile));
        }

        public static IList<CustomDataType> ParseSpecifiedDataTypes(IEnumerable<string> dtNames, string mibFile)
        {
            Regex rx = GetDataTypesRegex(dtNames);
            return ParseDataTypesRegexResults(rx.Matches(mibFile));
        }

        private static Regex GetDataTypesRegex(IEnumerable<string> dataTypesToMatch)
        {
            string namesToMatch = dataTypesToMatch.Any()
                ? string.Join("|", dataTypesToMatch)
                : @"\w+";

            return new Regex(
                string.Format(@"(?<{0}>{1})\s*::=\s*\[(?<{2}>\w+)\s*(?<{3}>[0-9]+)\].*?(?<{4}>{5})\s*(?<{6}>{7})\s*(\(.*?(?<{8}>[0-9]+)\)\)|\((?<{9}>[0-9]+)..(?<{10}>[0-9]+)\)|\s*?)",
                    DataTypeNameGrp,
                    namesToMatch,
                    DataTypeScopeGrp,
                    DataTypeIdGrp,
                    DataTypeClassGrp,
                    string.Join("|", Enum.GetNames(typeof(CustomDataType.ClassEnum))),
                    DataTypeBaseTypeGrp,
                    string.Join("|", SmiEnums.GetEnumCustomizedTextList<SmiEnums.DataTypeBase>()),
                    DataTypeSizeGrp,
                    DataTypeSizeMinGrp,
                    DataTypeSizeMaxGrp
                ),
                RegexOptions.Singleline);
        }

        private static IList<CustomDataType> ParseDataTypesRegexResults(MatchCollection regexResults)
        {
            var dataTypes = new List<CustomDataType>();

            foreach (Match match in regexResults)
            {
                string name = match.Groups[DataTypeNameGrp].Value;
                long id = long.Parse(match.Groups[DataTypeIdGrp].Value);
                SmiEnums.DataTypeScope scope = SmiEnums.Parse<SmiEnums.DataTypeScope>(
                    match.Groups[DataTypeScopeGrp].Value);
                CustomDataType.ClassEnum typeClass = (CustomDataType.ClassEnum)Enum.Parse(
                    typeof(CustomDataType.ClassEnum), match.Groups[DataTypeClassGrp].Value);
                SmiEnums.DataTypeBase baseType = SmiEnums.Parse<SmiEnums.DataTypeBase>(
                    match.Groups[DataTypeBaseTypeGrp].Value);
                string sizeRestriction = match.Groups[DataTypeSizeGrp].Value;
                string sizeRestrictionMin = match.Groups[DataTypeSizeMinGrp].Value;
                string sizeRestrictionMax = match.Groups[DataTypeSizeMaxGrp].Value;

                var newDataType = new CustomDataType()
                {
                    Name = name,
                    ID = id,
                    Scope = scope,
                    Class = typeClass,
                    BaseType = baseType,
                };

                if (string.IsNullOrEmpty(sizeRestriction))
                {
                    if (!string.IsNullOrEmpty(sizeRestrictionMin)
                        && !string.IsNullOrEmpty(sizeRestrictionMax))
                    {
                        newDataType.SizeRange = new Range<long>(
                            long.Parse(sizeRestrictionMin), long.Parse(sizeRestrictionMax));
                    }
                }
                else
                {
                    newDataType.Size = long.Parse(sizeRestriction);
                }

                dataTypes.Add(newDataType);
            }

            return dataTypes;
        }
    }
}
