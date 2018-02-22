using SmiParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parse mib file into tree
            MibData mib = MibParser.Parse("RFC1213-MIB");
            mib.MibTreeRoot.PrintToConsole("", true);
            
            //Select oid
            TreeNode found = null;
            while (found == null || found.ObjectType == null)
            {
                Console.WriteLine(">Enter OID");
                string mibToFind = Console.ReadLine();
                string[] indexPath = mibToFind.Split('.');
                Queue<long> pathQ = new Queue<long>(indexPath.Select(x => long.Parse(x)));
                try
                {
                    found = mib.MibTreeRoot.FindByIndex(pathQ);
                }
                catch (Exception e)
                {

                }

                if (found != null)
                {
                    
                    Console.WriteLine(found.RestrictionsDescription);
                }
                else
                    Console.WriteLine("Specified OID not found");
            }

            while (true)
            {
                //Read and validate value
                string value;
                do
                {
                    Console.WriteLine(">Enter value for selected OBJECT-TYPE:");
                    value = Console.ReadLine();
                }
                while (!found.Validate(value));

                byte[] encodedBytes = BerEncoding.Encoder.Encode(found.ObjectType.DataType, value).ToArray();
                Console.WriteLine(string.Join(" ",
                    encodedBytes.Select(x => Convert.ToString(x, 2).PadLeft(8, '0'))));

            }
        }
        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
    }
}
