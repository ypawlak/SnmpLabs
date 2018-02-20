using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using SmiParser.Utils;

namespace SmiParser
{
    public static class MibParser
    {
        public const string MibFilesPath = @"MibSources//";
        public static void Parse(string fileName)
        {
            string mibFileTxt = FileUtils.GetAllFileAsText(MibFilesPath, fileName);
            IEnumerable<ImportInfo> importsByFiles = ImportsParser.ParseImportInfo(mibFileTxt);
            foreach (ImportInfo singleFileImports in importsByFiles)
            {
                try
                {
                    Parse(singleFileImports.Filename);
                }
                catch (FileNotFoundException e)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("File {0} has not been found", e.Message));
                }
                
                //TODO: merge results
            }

            IEnumerable<CustomDataType> dataTypes = DataTypesParser.ParseAllDataTypes(mibFileTxt);
            IEnumerable<AliasInfo> aliases = AliasesParser.Parse(mibFileTxt);
            IEnumerable<OidInfo> oids = OidsParser.ParseAllOids(mibFileTxt);
            
        }

        //public static IEnumerable<CustomDataType> ParseImport(ImportInfo toImport, string sourceFilesPath)
        //{
        //    string mibFileTxt;
        //    try
        //    {
        //        mibFileTxt = FileUtils.GetAllFileAsText(sourceFilesPath, toImport.Filename);
        //    }
        //    catch(FileNotFoundException e)
        //    {
        //        System.Diagnostics.Debug.WriteLine(string.Format("File {0} has not been found: {1}",
        //            toImport.Filename, e.Message));
        //        return new List<CustomDataType>();
        //    }

        //    IEnumerable<CustomDataType> importedDts = new List<CustomDataType>();
        //    IEnumerable<ImportInfo> recImports = ImportsParser.ParseImportInfo(mibFileTxt);
        //    foreach (ImportInfo rImport in recImports)
        //        importedDts = importedDts.Concat(ParseImport(rImport, sourceFilesPath));

        //    return importedDts.Concat(
        //        DataTypesParser.ParseSpecifiedDataTypes(toImport.Terms, mibFileTxt));
        //}
    }
}
