using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using SmiParser.Utils;
using System.Diagnostics;

namespace SmiParser
{
    public static class MibParser
    {
        public const string MibFilesPath = @"MibSources//";
        public static MibData Parse(string fileName)
        {
            MibData result = InitMibData();

            string mibFileTxt = FileUtils.GetAllFileAsText(MibFilesPath, fileName);
            IEnumerable<ImportInfo> importsByFiles = ImportsParser.ParseImportInfo(mibFileTxt);
            foreach (ImportInfo singleFileImports in importsByFiles)
            {
                try
                {
                    MibData imported = Parse(singleFileImports.Filename);
                    result = Merge(result, imported);
                }
                catch (FileNotFoundException e)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("File {0} has not been found", e.Message));
                }
            }

            IEnumerable<CustomDataType> dataTypes = DataTypesParser.ParseAllDataTypes(mibFileTxt);
            foreach (CustomDataType type in dataTypes)
                result.DataTypes.Add(type.Name, type);

            IEnumerable<AliasInfo> parsedAliases = AliasesParser.Parse(mibFileTxt);
            IEnumerable<DataTypeAlias> aliases = parsedAliases
                .Select(
                    ai =>
                        GetAliasTypeFromInfo(result.DataTypes, ai))
                .Where(alias => alias != null)
                .ToList();

            foreach (DataTypeAlias alias in aliases)
                result.DataTypes.Add(alias.Alias, alias);

            IEnumerable<OidInfo> oids = OidsParser.ParseAllOids(mibFileTxt);
            foreach (OidInfo oi in oids)
            {
                var oiNode = new TreeNode()
                {
                    Children = new List<TreeNode>(),
                    Name = oi.Name,
                    SiblingIndex = oi.SiblingNo
                };
                TreeBuilder.InsertIntoTree(oiNode, oi.ParentExpression, result.FlatMibTree);
            }

            return result;
        }

        public static MibData InitMibData()
        {
            IDictionary<string, TreeNode> nodesByName = new Dictionary<string, TreeNode>();
            var mibTreeRoot = new TreeNode()
            {
                Children = new List<TreeNode>(),
                Name = "internet",
                SiblingIndex = 1
            };
            nodesByName.Add(mibTreeRoot.Name, mibTreeRoot);

            IDictionary<string, IDataType> baseDataTypes = Enum.GetValues(typeof(SmiEnums.DataTypeBase))
                .OfType<SmiEnums.DataTypeBase>()
                .ToList()
                .ToDictionary(
                    bt => SmiEnums.GetString(bt),
                    bt =>
                        (IDataType)new BaseDataType() { BaseType = bt, Name = SmiEnums.GetString(bt)});

            return new MibData()
            {
                DataTypes = baseDataTypes,
                MibTreeRoot = mibTreeRoot,
                FlatMibTree = nodesByName
            };
        }
        public static MibData Merge(MibData current, MibData imported)
        {
            foreach(var importedNode in imported.FlatMibTree.Where(n => n.Value.Parent != null))
            {
                //TODO: optimize for nested nodes from import
                TreeBuilder.InsertIntoTree(importedNode.Value, importedNode.Value.Parent.Name,
                    current.FlatMibTree);
            }

            foreach(var typeKV in imported.DataTypes.Where(type => !current.DataTypes.ContainsKey(type.Key)))
                    current.DataTypes.Add(typeKV.Key, typeKV.Value);

            return current;
        }

        public static DataTypeAlias GetAliasTypeFromInfo(IDictionary<string, IDataType> currentDataTypes,
            AliasInfo toInsert)
        {
            DataTypeAlias alias = new DataTypeAlias()
            {
                Alias = toInsert.Alias
            };

            if (!string.IsNullOrEmpty(toInsert.OriginalBaseTypeName) &&
                currentDataTypes.ContainsKey(toInsert.OriginalBaseTypeName))
            {
                alias.DataType = currentDataTypes[toInsert.OriginalBaseTypeName];
            }
            else if (!string.IsNullOrEmpty(toInsert.OriginalComplexTypeName) &&
                currentDataTypes.ContainsKey(toInsert.OriginalComplexTypeName))
            {
                alias.DataType = currentDataTypes[toInsert.OriginalComplexTypeName];
            }
            else
            {
                Debug.WriteLine(string.Format("Original DataType not found for alias: {0} {1} {2}"),
                    toInsert.Alias, toInsert.OriginalBaseTypeName, toInsert.OriginalComplexTypeName);
                return null;
            }

            return alias;
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
