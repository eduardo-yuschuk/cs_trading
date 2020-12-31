/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinancialData.Shared;
using DukascopyQuote.FinancialDataProvider;

namespace FinancialData.Manager.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void FinancialDataManagerTest()
        {
            Asset asset = new Asset("EUR/USD", AssetType.Currency);
            IDataSource source = new DukascopyDataSource();
            DukascopyOfflineProvider.Instance.AsyncGetHistory(
                asset,
                new DateTime(2000, 1, 1),
                new DateTime(2015, 1, 1),
                new DukascopyOfflineContext(@"C:\EURUSD_Ticks_2010.01.01_2017.12.19.csv", SampleType.Quote,
                    TimeSpan.Zero),
                (Func<ISample, bool>) (sample =>
                {
                    FinancialDataManager.Instance.AddQuote((IQuote) sample);
                    return true;
                }));
        }

        [TestMethod]
        public void FinancialDataBufferTest()
        {
            Asset asset = new Asset("EUR/USD", AssetType.Currency);
            IDataSource source = new DukascopyDataSource();
            FinancialDataBuffer buffer = new FinancialDataBuffer(10000);
            DukascopyOfflineProvider.Instance.AsyncGetHistory(
                asset,
                new DateTime(2000, 1, 1),
                new DateTime(2015, 1, 1),
                new DukascopyOfflineContext(@"C:\EURUSD_Ticks_2010.01.01_2017.12.19.csv", SampleType.Quote,
                    TimeSpan.Zero),
                (Func<ISample, bool>) (sample =>
                {
                    buffer.AddQuote((IQuote) sample);
                    return true;
                }));
        }
    }
}