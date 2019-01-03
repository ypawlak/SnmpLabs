using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SmiParser
{
    public static class AliasesParser
    {
        private const string AliasNameGrp = "aliasName";
        private const string AliasBTGrp = "aliasBT";
        private const string AliasCTGrp = "aliasCT";

        private static readonly Regex TypeAliasesRegex =
            new Regex(
                string.Format(@"^\s*(?<{0}>\w+)\s+::=\s+((?<{1}>{2})|(?<{3}>\w+)\s*\n)",
                    AliasNameGrp,
                    AliasBTGrp,
                    string.Join("|", SmiEnums.GetEnumCustomizedTextList<SmiEnums.DataTypeBase>()),
                    AliasCTGrp
                    ),
                RegexOptions.Singleline | RegexOptions.Multiline);
        public static IEnumerable<AliasInfo> Parse(string mibFileTxt)
        {
            var aliases = new List<AliasInfo>();

            foreach (Match match in TypeAliasesRegex.Matches(mibFileTxt))
            {
                string aliasName = match.Groups[AliasNameGrp].Value;
                string aliasedBaseType = match.Groups[AliasBTGrp].Value;
                string aliasedComplexType = match.Groups[AliasCTGrp].Value;

                aliases.Add(
                    new AliasInfo()
                    {
                        Alias = aliasName,
                        OriginalBaseTypeName = aliasedBaseType,
                        OriginalComplexTypeName = aliasedComplexType
                    });
            }

            return aliases;
        }
    }
}
