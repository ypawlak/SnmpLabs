using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SmiParser
{
    public static class OidsParser
    {
        const string OidNameGrp = "oName";
        const string OidParentGrp = "oParent";
        const string OidSiblingGrp = "oSibling";

        public static IEnumerable<OidInfo> ParseAllOids(string mibFile)
        {
            Regex rx = GetOidsRegex(new List<string>());
            return ParseOidsRegexResults(rx.Matches(mibFile));
        }

        public static IList<OidInfo> ParseSpecifiedOids(IEnumerable<string> dtNames, string mibFile)
        {
            Regex rx = GetOidsRegex(dtNames);
            return ParseOidsRegexResults(rx.Matches(mibFile));
        }

        private static Regex GetOidsRegex(IEnumerable<string> dataTypesToMatch)
        {
            string namesToMatch = dataTypesToMatch.Any()
                ? string.Join("|", dataTypesToMatch)
                : @"[^\s]+";

            return new Regex(
                string.Format(@"^\s*(?<{0}>{1}?)\s+OBJECT IDENTIFIER\s+::=\s?{{\s?(?<{2}>.*?)\s+?(?<{3}>[0-9]+)\s+?}}",
                    OidNameGrp,
                    namesToMatch,
                    OidParentGrp,
                    OidSiblingGrp
                ),
                RegexOptions.Singleline | RegexOptions.Multiline);
        }

        private static IList<OidInfo> ParseOidsRegexResults(MatchCollection regexResults)
        {
            var parsedOids = new List<OidInfo>();

            foreach (Match match in regexResults)
            {
                string name = match.Groups[OidNameGrp].Value;
                string parentExpression = match.Groups[OidParentGrp].Value;
                long sibllingNo = long.Parse(match.Groups[OidSiblingGrp].Value);

                parsedOids.Add(
                    new OidInfo()
                    {
                        Name = name,
                        SiblingNo = sibllingNo,
                        ParentExpression = parentExpression
                    });
            }

            return parsedOids;
        }
    }
}
