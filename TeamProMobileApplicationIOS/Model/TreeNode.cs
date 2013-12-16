using System;
using System.Collections.Generic;

namespace TeamProMobileApplicationIOS.Model
{
    public class TreeNode<T> where T : IComparable<T>
    {
        private List<TreeNode<T>> _children = new List<TreeNode<T>>();

        private TreeNode<T> _parent;

        // Add a child tree node

        public TreeNode<T> Add(T child)
        {
            TreeNode<T> newNode = new TreeNode<T> { Node = child };
            _children.Add(newNode);
            newNode._parent = this;
            return newNode;
        }

        // Remove a child tree node
        public void Remove(T child)
        {
            foreach (TreeNode<T> treeNode in _children)
            {
                if (treeNode.Node.CompareTo(child) == 0)
                {
                    _children.Remove(treeNode);
                    return;
                }
            }
        }

        public TreeNode<T> Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        // Gets or sets the node
        public T Node { get; set; }

        // Gets treenode children
        public List<TreeNode<T>> Children
        {
            get { return _children; }
        }

        // Recursively displays node and its children 
        public static void Display(TreeNode<T> node, int indentation)
        {
            string line = new String('-', indentation);
            Console.WriteLine(line + " " + node.Node);

            foreach (TreeNode<T> treeNode in node.Children)
            {
                Display(treeNode, indentation + 1);
            }
        }
    }
}
