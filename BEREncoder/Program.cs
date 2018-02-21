using SmiParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BerEncoder
{
    class Program
    {
        static void Main(string[] args)
        {
            MibData mib = MibParser.Parse("RFC1213-MIB");
            mib.MibTreeRoot.PrintToConsole("", true);

            
            TreeNode found = null;
            while (found == null)
            {
                Console.WriteLine("Enter OID");
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
                    Console.WriteLine(found.DisplayName);
                else
                    Console.WriteLine("Specified OID not found");
            }
        }
    }
}
