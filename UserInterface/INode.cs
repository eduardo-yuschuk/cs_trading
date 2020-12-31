/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;

namespace UserInterface
{
    public interface INode
    {
        string Label { get; }
        List<INode> Nodes { get; }
    }
}