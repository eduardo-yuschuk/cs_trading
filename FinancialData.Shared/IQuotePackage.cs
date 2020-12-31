/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace FinancialData.Shared
{
    public interface IQuotePackage : ISamplePackage<IQuote>
    {
        Asset Asset { get; }
        IDataSource Source { get; }
    }
}