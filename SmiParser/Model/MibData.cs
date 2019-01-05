using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SmiParser.Model
{
    public class MibData
    {
        public IDictionary<string, IDataType> DataTypes { get; set; }
        public TreeNode MibTreeRoot { get; set; }
        public IDictionary<string, TreeNode> FlatMibTree { get; set; }
    }
}
