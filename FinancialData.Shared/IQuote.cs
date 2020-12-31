/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialData.Shared
{
    public interface IQuote : ISample
    {
        Asset Asset { get; }
        IDataSource Source { get; }
        DateTime DateTime { get; }
        decimal Ask { get; }
        decimal AskSize { get; }
        decimal Bid { get; }
        decimal BidSize { get; }
    }
}