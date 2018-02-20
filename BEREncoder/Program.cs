using SmiParser;
using System;

namespace BerEncoder
{
    class Program
    {
        static void Main(string[] args)
        {
            MibData mib = MibParser.Parse("RFC1213-MIB");
            mib.MibTreeRoot.PrintToConsole("", true);
        }
    }
}
