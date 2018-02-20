using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SmiParser
{
    class ObjectTypesParser
    {
        private readonly Regex objTypesGeneralParser = new Regex(
            @"^(?<objTypeName>\w+)\s+OBJECT-TYPE\s+(?<objTypeDesc>.*?)\s+::=\s+{\s?(?<oidParentExp>.*?)\s+?(?<siblingNum>[0-9]+)\s+?}");
        private readonly Regex objTypesParserSyntax = new Regex(
            @"^\s*SYNTAX\s*(?<objTypeSyntax>INTEGER|OCTET STRING|OBJECT IDENTIFIER|NULL|SEQUENCE|\w+)\s*(\(SIZE\s*\((?<objTypeSize>[0-9]+)\)\)|((\(SIZE\s+)|())\((?<objTypeSizeMin>[0-9]+)..(?<objTypeSizeMax>[0-9]+)\)|\s*?)");

    }
}
