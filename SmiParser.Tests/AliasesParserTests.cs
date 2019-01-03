using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using Xunit;
using Xunit.Sdk;
using SmiParser;
using SmiParser.Info;

namespace SmiParser.Tests
{
    public class AliasesParserTests
    {
        [Fact]
        public void EmptyTest()
        {
            IEnumerable<AliasInfo> emptyResult = AliasesParser.Parse(string.Empty);
            Assert.NotNull(emptyResult);
            Assert.Empty(emptyResult);
        }

        public static IEnumerable<object[]> ExampleMibCases =>
        new List<object[]>
        {
            new object[] {
                @"
DisplayString ::=
    OCTET STRING",
                new List<AliasInfo>
                {
                    new AliasInfo
                        {
                            Alias = "DisplayString",
                            OriginalBaseTypeName = "OCTET STRING",
                            OriginalComplexTypeName = string.Empty 
                        }
                }
            }
        };

        [Theory]
        [MemberData(nameof(ExampleMibCases))]
        public void ExampleMibsAliasesParseTest(string input, List<AliasInfo> expectedResult)
        {
            IEnumerable<AliasInfo> executionResult = AliasesParser.Parse(input);
            Assert.Equal(expectedResult, executionResult, new AliasInfoValueEqualityComparer());
        }
    }
}