using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using Xunit;
using Xunit.Sdk;
using SmiParser;
using SmiParser.Info;
using SmiParser.Parsers;

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


        [Theory]
        [MemberData(nameof(IsolatedTypesCases))]
        public void IsolatedTypesTest(string input, AliasInfo expected)
        {
            IEnumerable<AliasInfo> executionResult = AliasesParser.Parse(input);
            Assert.Single(executionResult);
            Assert.Equal(expected, executionResult.Single(), new AliasInfoValueEqualityComparer());
        }

        public static IEnumerable<object[]> IsolatedTypesCases =>
        new List<object[]>
        {
            new object[] {
                "DisplayString ::= OCTET STRING",
                new AliasInfo
                    {
                        Alias = "DisplayString",
                        OriginalBaseTypeName = "OCTET STRING",
                        OriginalComplexTypeName = string.Empty 
                    }
            },
            new object[] {
                @"
                    IpAddrEntry ::=
                        SEQUENCE {
                            ipAdEntAddr
                                IpAddress,
                            ipAdEntIfIndex
                                INTEGER,
                            ipAdEntNetMask
                                IpAddress,
                            ipAdEntBcastAddr
                                INTEGER,
                            ipAdEntReasmMaxSize
                                INTEGER (0..65535)
                }",
                new AliasInfo
                        {
                            Alias = "IpAddrEntry",
                            OriginalBaseTypeName = "SEQUENCE",
                            OriginalComplexTypeName = string.Empty 
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

        public static IEnumerable<object[]> ExampleMibCases =>
        new List<object[]>
        {
            new object[] {
                @"RFC1155-SMI DEFINITIONS ::= BEGIN

                    EXPORTS -- EVERYTHING
                            internet, directory, mgmt,
                            experimental, private, enterprises,
                            OBJECT-TYPE, ObjectName, ObjectSyntax, SimpleSyntax,
                            ApplicationSyntax, NetworkAddress, IpAddress,
                            Counter, Gauge, TimeTicks, Opaque;

                    -- the path to the root

                    internet      OBJECT IDENTIFIER ::= { iso org(3) dod(6) 1 }

                    directory     OBJECT IDENTIFIER ::= { internet 1 }

                    mgmt          OBJECT IDENTIFIER ::= { internet 2 }

                    experimental  OBJECT IDENTIFIER ::= { internet 3 }

                    private       OBJECT IDENTIFIER ::= { internet 4 }
                    enterprises   OBJECT IDENTIFIER ::= { private 1 }

                    -- definition of object types

                    OBJECT-TYPE MACRO ::=
                    BEGIN
                        TYPE NOTATION ::= ""SYNTAX"" type (TYPE ObjectSyntax)
                                        ""ACCESS"" Access
                                        ""STATUS"" Status
                        VALUE NOTATION ::= value (VALUE ObjectName)

                        Access ::= ""read-only""
                                        | ""read-write""
                                        | ""write-only""
                                        | ""not-accessible""
                        Status ::= ""mandatory""
                                        | ""optional""
                                        | ""obsolete""
                    END

                        -- names of objects in the MIB

                        ObjectName ::=
                            OBJECT IDENTIFIER

                        -- syntax of objects in the MIB

                        ObjectSyntax ::=
                            CHOICE {
                                simple
                                    SimpleSyntax,
                        -- note that simple SEQUENCEs are not directly
                        -- mentioned here to keep things simple (i.e.,
                        -- prevent mis-use).  However, application-wide
                        -- types which are IMPLICITly encoded simple
                        -- SEQUENCEs may appear in the following CHOICE

                                application-wide
                                    ApplicationSyntax
                            }

                        SimpleSyntax ::=
                            CHOICE {
                                number
                                    INTEGER,
                                string
                                    OCTET STRING,
                                object
                                    OBJECT IDENTIFIER,
                                empty
                                    NULL
                            }

                        ApplicationSyntax ::=
                            CHOICE {
                                address
                                    NetworkAddress,
                                counter
                                    Counter,
                                gauge
                                    Gauge,
                                ticks
                                    TimeTicks,
                                arbitrary
                                    Opaque

                        -- other application-wide types, as they are
                        -- defined, will be added here
                            }

                        -- application-wide types

                        NetworkAddress ::=
                            CHOICE {
                                internet
                                    IpAddress
                            }

                        IpAddress ::=
                            [APPLICATION 0]          -- in network-byte order
                                IMPLICIT OCTET STRING (SIZE (4))

                        Counter ::=
                            [APPLICATION 1]
                                IMPLICIT INTEGER (0..4294967295)

                        Gauge ::=
                            [APPLICATION 2]
                                IMPLICIT INTEGER (0..4294967295)

                        TimeTicks ::=
                            [APPLICATION 3]
                                IMPLICIT INTEGER (0..4294967295)

                        Opaque ::=
                            [APPLICATION 4]          -- arbitrary ASN.1 value,
                                IMPLICIT OCTET STRING   --   ""double-wrapped""

                        END
                    ",
                new List<AliasInfo>
                {
                    new AliasInfo
                        {
                            Alias = "ObjectName",
                            OriginalBaseTypeName = "OBJECT IDENTIFIER",
                            OriginalComplexTypeName = string.Empty 
                        }
                }
            }
        };
    }
}