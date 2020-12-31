/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using FinancialData.Shared;
using FinancialData;
using System.Globalization;

namespace DukascopyQuote.FinancialDataProvider
{
    public class DukascopyOfflineProvider : IFinancialDataProvider
    {
        private static CultureInfo _culture = CultureInfo.GetCultureInfo("en-US");

        #region Singleton

        private static DukascopyOfflineProvider _instance;
        private static object _instanceLock = new object();

        public static DukascopyOfflineProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DukascopyOfflineProvider();
                        }
                    }
                }

                return _instance;
            }
        }

        private DukascopyOfflineProvider()
        {
        }

        #endregion

        public ISample GetPrice(Asset asset)
        {
            throw new NotImplementedException();
        }

        private Bar BuildBar(Asset asset, IDataSource source, TimeSpan timeFrame, string text)
        {
            // Time,Open,High,Low,Close,Volume
            // 01.01.2014 00:00:00.000,1.06180,1.06180,1.06180,1.06180,0.00
            var parts = text.Split(new char[] {','});
            return new Bar(asset, source, timeFrame,
                DateTime.ParseExact(parts[0], "dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture),
                decimal.Parse(parts[1], _culture), decimal.Parse(parts[2], _culture), decimal.Parse(parts[3], _culture),
                decimal.Parse(parts[4], _culture), (long) decimal.Parse(parts[5], _culture));
        }

        public ISamplePackage GetHistory(Asset asset, DateTime start, DateTime end, IProvisionContext provisionContext)
        {
            List<string> data = DukascopyOfflineReader.GetHistoricalPrices(provisionContext.Source);
            switch (provisionContext.SampleType)
            {
                case SampleType.Bar:
                    // first line must be discarded (titles)
                    data.RemoveAt(0);
                    TimeSpan period = provisionContext.Period;
                    IDataSource source = new DukascopyDataSource();
                    return new BarPackage
                    {
                        Asset = asset,
                        Source = source,
                        Period = period,
                        Samples = data.Select(line => (IBar) BuildBar(asset, source, period, line)).ToList()
                    };
                case SampleType.Quote:
                default:
                    throw new NotImplementedException();
            }
        }

        public void AsyncGetHistory(Asset asset, DateTime start, DateTime end, IProvisionContext provisionContext,
            Func<ISample, bool> func)
        {
            IDataSource source = new DukascopyDataSource();
            DukascopyOfflineReader.GetHistoricalPrices(
                provisionContext.Source,
                (Func<string, bool>) (line => { return func(Quote.From5PartsString(line, asset, source)); }));
        }
    }
}