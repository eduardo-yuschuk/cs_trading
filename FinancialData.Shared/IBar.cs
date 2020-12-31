/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialData.Shared
{
    public interface IBar : ISample
    {
        Asset Asset { get; }
        IDataSource Source { get; }
        DateTime DateTime { get; }
        decimal Open { get; }
        decimal High { get; }
        decimal Low { get; }
        decimal Close { get; }
        long Volume { get; }
        decimal AdjClose { get; }
    }
}