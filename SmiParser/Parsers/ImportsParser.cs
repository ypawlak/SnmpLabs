using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using SmiParser.Info;

namespace SmiParser.Parsers
{
    public static class ImportsParser
    {
        private const string WholeImportGrp = "import";

        private static readonly Regex WholeImportRegex =
            new Regex(
                string.Format(
                    "IMPORTS(?<{0}>.*?;)", WholeImportGrp),
                RegexOptions.Singleline);

        private const string ImportTermsGrp = "importTerms";
        private const string ImportSourceGrp = "importSource";

        private static readonly Regex ImportsSplittedByFilesRegex =
            new Regex(
                string.Format(@"(?<{0}>.*?)FROM\s*(?<{1}>.*?)[\s;]",
                    ImportTermsGrp, ImportSourceGrp),
                RegexOptions.Singleline);
        public static IEnumerable<ImportInfo> ParseImportInfo(string mibFileTxt)
        {
            var imports = new List<ImportInfo>();

            Match wholeImportTxt = WholeImportRegex.Match(mibFileTxt);
            string importByFilesTxt = wholeImportTxt.Groups[WholeImportGrp].Value;

            foreach (Match match in ImportsSplittedByFilesRegex.Matches(importByFilesTxt))
            {
                string termsMatchedValue = match.Groups[ImportTermsGrp].Value;
                IEnumerable<string> termsToImport = termsMatchedValue
                    .Split(',')
                    .Select(term =>
                        term.Trim());
                string sourceFileMatchedValue = match.Groups[ImportSourceGrp].Value;

                imports.Add(new ImportInfo { Filename = sourceFileMatchedValue, Terms = termsToImport });
            }

            return imports;
        }
    }
}
