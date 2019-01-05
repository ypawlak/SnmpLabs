using SmiParser.Model;
using SmiParser.Utils;
using System;
using System.IO;
using System.Linq;

namespace SmiParser
{
    class Program
    {
        static void Main(string[] args)
        {
            MibData mib = MibParser.Parse("RFC1213-MIB");
            mib.MibTreeRoot.PrintToConsole("", true);

            //System.Diagnostics.Debug.WriteLine(
            //    string.Join("|", SmiEnums.GetEnumCustomizedTextList<SmiEnums.ObjectTypeAccess>()));
            //System.Diagnostics.Debug.WriteLine(
            //    string.Join("|", SmiEnums.GetEnumCustomizedTextList<SmiEnums.ObjectTypeStatus>()));

            //string DataTypeNameGrp = "dtName";
            //string DataTypeScopeGrp = "dtScope";
            //string DataTypeIdGrp = "dtId";
            //string DataTypeClassGrp = "dtClass";
            //string DataTypeBaseTypeGrp = "dtBaseType";
            //string DataTypeSizeGrp = "dtSize";
            //string DataTypeSizeMinGrp = "dtSizeMin";
            //string DataTypeSizeMaxGrp = "dtSizeMax";
            //System.Diagnostics.Debug.WriteLine(
            //    string.Format(@"(?<{0}>{1})\s*::=\s*\[(?<{2}>\w+)\s*(?<{3}>[0-9]+)\].*?(?<{4}>{5})\s*(?<{6}>{7})\s*(\(.*?(?<{8}>[0-9]+)\)\)|\((?<{9}>[0-9]+)..(?<{10}>[0-9]+)\)|\s*?)",
            //        DataTypeNameGrp,
            //        @"\w+",
            //        DataTypeScopeGrp,
            //        DataTypeIdGrp,
            //        DataTypeClassGrp,
            //        string.Join("|", Enum.GetNames(typeof(CustomDataType.ClassEnum))),
            //        DataTypeBaseTypeGrp,
            //        string.Join("|", SmiEnums.GetEnumCustomizedTextList<SmiEnums.DataTypeBase>()),
            //        DataTypeSizeGrp,
            //        DataTypeSizeMinGrp,
            //        DataTypeSizeMaxGrp));
            //Console.WriteLine(SmiEnums.Parse<SmiEnums.ObjectTypeAccess>("read-only"));

            //Console.WriteLine(SmiEnums.ObjectTypeStatus.mandatory.ToString());
            //Console.WriteLine(SmiEnums.Parse<SmiEnums.ObjectTypeStatus>("mandatory"));

            //MibParser.Parse(@"MibSources//", "RFC1155-SMI");
            //Console.WriteLine(MibType.Parse("CONTEXT-SPECIFIC"));
            //Console.WriteLine(MibType.GetString(MibType.Parse("CONTEXT-SPECIFIC")));
            //Console.WriteLine(MibType.Parse("APPLICATION"));
            //Console.WriteLine(MibType.GetString(MibType.Parse("APPLICATION")));
            //string DataTypeNameGrp = "dtName";
            //string DataTypeScopeGrp = "dtScope";
            //string DataTypeIdGrp = "dtId";
            //string DataTypeClassGrp = "dtClass";
            //string DataTypeBaseTypeGrp = "dtBaseType";
            //string DataTypeSize = "dtSize";
            //string DataTypeSizeMin = "dtSizeMin";
            //string DataTypeSizeMax = "dtSizeMax";
            //System.Diagnostics.Debug.WriteLine(string.Format(@"(?<{0}>\w+)\s*::=\s*\[(?<{1}>\w+)\s*(?<{2}>[0-9]+)\].*?(?<{3}>{4})\s*(?<{5}>{6})\s*(\(.*?(?<{7}>[0-9]+)\)\)|\((?<{8}>[0-9]+)..(?<{9}>[0-9]+)\)|\s*?)",
            //        DataTypeNameGrp,
            //        DataTypeScopeGrp,
            //        DataTypeIdGrp,
            //        DataTypeClassGrp,
            //        string.Join("|", Enum.GetNames(typeof(SmiType.ClassEnum))),
            //            DataTypeBaseTypeGrp,
            //            string.Join("|", SmiEnums.GetEnumCustomizedTextList<SmiEnums.BaseType>()),
            //            DataTypeSize,
            //            DataTypeSizeMin,
            //            DataTypeSizeMax));
            //tests
            //var btList = SmiEnums.GetAllEnumStrings<SmiEnums.BaseTypes>();
            //var scpList = SmiEnums.GetAllEnumStrings<SmiEnums.Scope>();
            //var test = MibParser.SmiDataTypeRegex.GetGroupNames();

            //string fTxt = FileUtils.GetAllFileAsText(@"MibSources//", "RFC1155-SMI");
            //var matches = MibParser.SmiDataTypeRegex.Matches(fTxt);
        }
    }
}
