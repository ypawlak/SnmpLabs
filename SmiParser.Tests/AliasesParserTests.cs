using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using Xunit;
using SmiParser;

namespace SmiParser.Tests
{
    public class AliasesParserTests
    {
        [Fact]
        public void EmptyTest()
        {
            IEnumerable<AliasInfo> emptyResult = AliasesParser.Parse("");
            Assert.NotNull(emptyResult);
            Assert.Empty(emptyResult);
        }
    }
}