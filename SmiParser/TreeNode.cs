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

        #region Print
        public string DisplayName
        {
            get
            {
                return string.Format("{0} ({1})", Name, SiblingIndex);
            }
        }

        public void PrintToConsole(string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }
            Console.WriteLine(DisplayName);

            for (int i = 0; i < Children.Count; i++)
                Children[i].PrintToConsole(indent, i == Children.Count - 1);
        }
        #endregion
    }
}
