/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using FinancialData.Manager.Shared;
using FinancialData.Shared;
using Context.Shared;

namespace FinancialData.Manager
{
    public class FinancialDataManager : IFinancialDataStore
    {
        public Dictionary<Asset, List<IDataSource>> AssetDataSources { get; set; }

        #region Singleton

        private static FinancialDataManager _instance;
        private static object _instanceLock = new object();

        public static FinancialDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FinancialDataManager();
                        }
                    }
                }

                return _instance;
            }
        }

        private FinancialDataManager()
        {
            AssetDataSources = new Dictionary<Asset, List<IDataSource>>();
        }

        #endregion

        #region Assets

        public List<Asset> GetAssets()
        {
            return AssetDataSources.Keys.ToList();
        }

        public List<Asset> GetAssets(IDataSource source)
        {
            return AssetDataSources
                .Where(pair => pair.Value.Contains(source))
                .Select(pair => pair.Key)
                .ToList();
        }

        #endregion

        #region DataSources

        public List<IDataSource> GetDataSources()
        {
            return AssetDataSources.SelectMany(pair => pair.Value).Distinct().ToList();
        }

        public List<IDataSource> GetDataSources(Asset asset)
        {
            return AssetDataSources[asset];
        }

        #endregion

        #region Quotes

        public void AddQuote(IQuote quote)
        {
            if (!AssetDataSources.ContainsKey(quote.Asset))
                AssetDataSources[quote.Asset] = new List<IDataSource>();
            if (!AssetDataSources[quote.Asset].Contains(quote.Source))
                AssetDataSources[quote.Asset].Add(quote.Source);
            StoreQuote(quote);
        }

        public void AddQuotes(List<IQuote> quotes, bool preserveOldData)
        {
            var research = ContextFactory.GetContext(UniqueContextType.Research);
            var historicalData = ContextFactory.GetSubcontext(research, ContextType.HistoricalData);
        }

        private void StoreQuote(IQuote quote)
        {
            throw new NotImplementedException();
        }

        public void ClearQuotes()
        {
            throw new NotImplementedException();
        }

        public void ClearQuotes(Asset asset)
        {
            throw new NotImplementedException();
        }

        public void ClearQuotes(Asset asset, IDataSource source)
        {
            throw new NotImplementedException();
        }

        public void ClearQuotes(IDataSource source)
        {
            throw new NotImplementedException();
        }

        public int GetQuotesCount()
        {
            throw new NotImplementedException();
        }

        public int GetQuotesCount(Asset asset)
        {
            throw new NotImplementedException();
        }

        public int GetQuotesCount(Asset asset, IDataSource source)
        {
            throw new NotImplementedException();
        }

        public int GetQuotesCount(IDataSource source)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}