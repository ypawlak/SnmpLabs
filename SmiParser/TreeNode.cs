using System;
using System.Collections.Generic;
using System.Text;

namespace SmiParser
{
    public class TreeNode
    {
        public TreeNode Parent { get; set; }
        public IList<TreeNode> Children { get; set; }

        public string Name { get; set; }
        public long SiblingIndex { get; set; }
        public ObjectType ObjectType { get; set; }
    }
}
