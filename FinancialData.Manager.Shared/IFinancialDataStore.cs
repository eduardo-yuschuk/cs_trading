/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using FinancialData.Shared;

namespace FinancialData.Manager.Shared
{
    public interface IFinancialDataStore
    {
        List<Asset> GetAssets();
        List<Asset> GetAssets(IDataSource source);
        List<IDataSource> GetDataSources();
        List<IDataSource> GetDataSources(Asset asset);

        void AddQuote(IQuote quote);
        void AddQuotes(List<IQuote> quotes, bool preserveOldData);

        void ClearQuotes();
        void ClearQuotes(Asset asset);
        void ClearQuotes(Asset asset, IDataSource source);
        void ClearQuotes(IDataSource source);

        int GetQuotesCount();
        int GetQuotesCount(Asset asset);
        int GetQuotesCount(Asset asset, IDataSource source);
        int GetQuotesCount(IDataSource source);
    }
}