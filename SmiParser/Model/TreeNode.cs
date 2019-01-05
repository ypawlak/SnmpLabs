using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SmiParser.Model
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
                return string.Format("{0} ({1}) {2}", Name, SiblingIndex,
                    ObjectType != null
                        ? ObjectType.DisplayName
                        : string.Empty);
            }
        }

        public string RestrictionsDescription
        {
            get
            {
                string desc = Name + Environment.NewLine +
                    (ObjectType == null
                        ? "No OBJECT-TYPE has been set"
                        : ObjectType.RestrictionsDescrption);
                return desc;
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

        public bool Validate(string value)
        {
            if (ObjectType == null)
                return false;
            return ObjectType.Validate(value);

        }

        public TreeNode FindByIndex(Queue<long> indexPath)
        {
            if(indexPath.Any())
            {
                if(indexPath.Peek() == SiblingIndex)
                {
                    indexPath.Dequeue();
                    if (indexPath.Any())
                    {
                        foreach(TreeNode child in Children)
                        {
                            TreeNode found = child.FindByIndex(indexPath);
                            if (found != null)
                                return found;
                        }
                        throw new Exception("Node could not be found");
                    }
                    else
                        return this;
                }
            }
            return null;
        }
    }
}
