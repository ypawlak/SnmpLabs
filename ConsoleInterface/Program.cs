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
                catch (Exception e) { }

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
                Console.WriteLine(ByteArrayToBinaryString(encodedBytes));
                Console.WriteLine(ByteArrayToHexString(encodedBytes));
                BerEncoding.DecodedObjectMeta decoded  = BerEncoding.Decoder.DecodeObject(encodedBytes);
                Console.WriteLine("> Enter hex string to decode");
                string hexBytes = Console.ReadLine();
                byte[] toDecode = HexStringToByteArray(hexBytes);
                decoded = BerEncoding.Decoder.DecodeObject(toDecode);
            }
        }

        private static string ByteArrayToBinaryString(byte[] byteArr)
        {
            return string.Join(" ",
                    byteArr.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));
        }

        public static string ByteArrayToHexString(byte[] byteArr)
        {
            string hex = BitConverter.ToString(byteArr);
            return hex.Replace("-", "");
        }
        public static byte[] HexStringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
