/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace Charts.Common
{
    public interface IDrawable
    {
        double MinValue { get; }
        double MaxValue { get; }
        DateTime Begin { get; }
        DateTime End { get; }
        IStructuredInformation Information { get; }
    }
}