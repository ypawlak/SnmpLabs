using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SmiParser
{
    class ObjectTypesParser
    {
        const string OtNameGrp = "objTypeName";
        const string OtSpecificationGrp = "objTypeSpec";
        const string OtParentGrp = "oidParentExp";
        const string OtSiblingNoGrp = "siblingNum";

        private readonly Regex objTypesGeneralParser = new Regex(
            @"^(?<objTypeName>\w+)\s+OBJECT-TYPE\s+(?<objTypeDesc>.*?)\s+::=\s+{\s?(?<oidParentExp>.*?)\s+?(?<siblingNum>[0-9]+)\s+?}",
            RegexOptions.Singleline | RegexOptions.Multiline);
        private readonly Regex objTypesParserSyntax = new Regex(
            @"^\s*SYNTAX\s*(?<objTypeSyntax>INTEGER|OCTET STRING|OBJECT IDENTIFIER|NULL|SEQUENCE|\w+)\s*(\(SIZE\s*\((?<objTypeSize>[0-9]+)\)\)|((\(SIZE\s+)|())\((?<objTypeSizeMin>[0-9]+)..(?<objTypeSizeMax>[0-9]+)\)|\s*?)");
        public static IDictionary<OidInfo, ObjectType> ParseAllObjectTypes(string mibFile)
        {
            Regex rx = GetObjTypesRegex();
            return ParseObjTypesRegexResults(rx.Matches(mibFile));
        }

        //public static IList<OidInfo> ParseSpecifiedObjTypes(IEnumerable<string> otNames, string mibFile)
        //{
        //    Regex rx = GetOidsRegex(otNames);
        //    return ParseOidsRegexResults(rx.Matches(mibFile));
        //}

        private static Regex GetObjTypesRegex()
        {
            return new Regex(
                string.Format(@"^(?<{0}>\w+)\s+OBJECT-TYPE\s+(?<{1}>.*?)\s+::=\s+{{\s?(?<{2}>.*?)\s+?(?<{3}>[0-9]+)\s+?}}",
                    OtNameGrp,
                    OtSpecificationGrp,
                    OtParentGrp,
                    OtSiblingNoGrp
                ),
                RegexOptions.Singleline | RegexOptions.Multiline);
        }

        private static IDictionary<OidInfo, ObjectType> ParseObjTypesRegexResults(MatchCollection regexResults)
        {
            var parsedObjTypes = new Dictionary<OidInfo, ObjectType>();

            foreach (Match match in regexResults)
            {
                string name = match.Groups[OtNameGrp].Value;
                string parentExpression = match.Groups[OtParentGrp].Value;
                string otSpecification = match.Groups[OtSpecificationGrp].Value;
                long sibllingNo = long.Parse(match.Groups[OtSiblingNoGrp].Value);

                var oid = new OidInfo()
                {
                    Name = name,
                    SiblingNo = sibllingNo,
                    ParentExpression = parentExpression
                };

                ObjectType parsedType = ParseObjectTypeFromSpecification(otSpecification);

                parsedObjTypes.Add(oid, parsedType);
            }

            return parsedObjTypes;
        }

        private static ObjectType ParseObjectTypeFromSpecification(string specification)
        {
            //TODO: parse object types specification
            return null;
        }
    }
}
