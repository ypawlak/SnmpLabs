using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using SmiParser.Model;

namespace SmiParser
{
    class TreeBuilder
    {
        private const string TreeRootStr =  "iso org(3) dod(6)";
        

        public static void InsertIntoTree(TreeNode toInsert, string parentExpression,
            IDictionary<string, TreeNode> treeDict)
        {
            if(treeDict.ContainsKey(toInsert.Name) &&
                treeDict.ContainsKey(parentExpression) &&
                treeDict[parentExpression].Equals(treeDict[toInsert.Name].Parent))
            {
                Debug.WriteLine(string.Format("Tree already contains the node with name: {0} and parent name: {1}",
                    toInsert.Name, parentExpression));
            }

            if (treeDict.ContainsKey(parentExpression))
            {
                TreeNode parentNode = treeDict[parentExpression];
                if (!parentNode.Children.Any(ch => ch.Name.Equals(toInsert.Name)))
                {
                    parentNode.Children.Add(toInsert);
                    toInsert.Parent = parentNode;
                }
                treeDict.Add(toInsert.Name, toInsert);
            }
            else
            {
                Debug.WriteLine(string.Format("Unable to find parent in given tree: {0}",
                    parentExpression));
            }
        }
    }
}
