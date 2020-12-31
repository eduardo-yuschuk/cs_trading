/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;

namespace UserInterface.Nodes
{
    class Node : INode
    {
        private string _label;
        private List<INode> _nodes = new List<INode>();

        public string Label
        {
            get { return _label; }
        }

        public List<INode> Nodes
        {
            get { return _nodes; }
        }

        public Node(string label)
        {
            _label = label;
        }

        public void AddNode(INode node)
        {
            _nodes.Add(node);
        }
    }
}