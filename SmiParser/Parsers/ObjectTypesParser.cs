using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using SmiParser.Utils;
using SmiParser.Info;

namespace SmiParser.Parsers
{
    class ObjectTypesParser
    {
        const string OtNameGrp = "objTypeName";
        const string OtSpecificationGrp = "objTypeSpec";
        const string OtParentGrp = "oidParentExp";
        const string OtSiblingNoGrp = "siblingNum";

        const string OtSyntaxGrp = "objTypeSyntax";
        const string OtSizeGrp = "objTypeSize";
        const string OtSizeMinGrp = "objTypeSizeMin";
        const string OtSizeMaxGrp = "objTypeSizeMax";

        public static IDictionary<OidInfo, ObjectTypeInfo> ParseAllObjectTypes(string mibFile)
        {
            Regex rx = GetObjTypesRegex();
            return ParseObjTypesRegexResults(rx.Matches(mibFile));
        }

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

        private static IDictionary<OidInfo, ObjectTypeInfo> ParseObjTypesRegexResults(MatchCollection regexResults)
        {
            var parsedObjTypes = new Dictionary<OidInfo, ObjectTypeInfo>();

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

                ObjectTypeInfo parsedType = ParseObjectTypeFromSpecification(otSpecification);

                parsedObjTypes.Add(oid, parsedType);
            }

            return parsedObjTypes;
        }

        private static ObjectTypeInfo ParseObjectTypeFromSpecification(string specification)
        {
            IList<ObjectTypeInfo> objTypeInfos = new List<ObjectTypeInfo>();
            foreach (Match match in GetObjectTypeSpecificsRegex().Matches(specification))
            {
                var objTypeI = new ObjectTypeInfo();
                objTypeI.Syntax = match.Groups[OtSyntaxGrp].Value;
                string sizeRestriction = match.Groups[OtSizeGrp].Value;
                string sizeRestrictionMin = match.Groups[OtSizeMinGrp].Value;
                string sizeRestrictionMax = match.Groups[OtSizeMaxGrp].Value;
                
                if (string.IsNullOrEmpty(sizeRestriction))
                {
                    if (!string.IsNullOrEmpty(sizeRestrictionMin)
                        && !string.IsNullOrEmpty(sizeRestrictionMax))
                    {
                        objTypeI.SyntaxSizeRange = new Range<long>(
                            long.Parse(sizeRestrictionMin), long.Parse(sizeRestrictionMax));
                    }
                }
                else
                {
                    objTypeI.SyntaxSize = long.Parse(sizeRestriction);
                }

                //TODO: parse the rest of object types specification

                objTypeInfos.Add(objTypeI);
            }
            return objTypeInfos.Single();
        }

        private static Regex GetObjectTypeSpecificsRegex()
        {
            //TODO: match access, status and description
            return new Regex(
                string.Format(@"^\s*SYNTAX\s*(?<{0}>INTEGER|OCTET STRING|OBJECT IDENTIFIER|NULL|SEQUENCE|\w+)\s*(\(SIZE\s*\((?<{1}>[0-9]+)\)\)|((\(SIZE\s+)|())\((?<{2}>[0-9]+)..(?<{3}>[0-9]+)\)|\s*?)",
                        OtSyntaxGrp,
                        OtSizeGrp,
                        OtSizeMinGrp,
                        OtSizeMaxGrp
                    ),
                    RegexOptions.Singleline | RegexOptions.Multiline
                );
        }
    }
}
