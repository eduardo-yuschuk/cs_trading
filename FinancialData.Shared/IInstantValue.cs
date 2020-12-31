/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialData.Shared
{
    public interface IInstantValue<T> : ISample
    {
        DateTime DateTime { get; }
        T Value { get; }
        T NormalizedValue { get; set; }
        IInstantValue<T> Clone();
    }
}