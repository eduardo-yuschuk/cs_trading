/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using DukascopyQuote.FinancialDataProvider;
using FinancialData.Shared;
using FinancialIndicator;
using FinancialIndicator.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FinancialData.Manager.IntegrationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.TestSMAOnRealData();
        }

        public void TestSMAOnRealData()
        {
            var prices = new List<decimal>();
            Asset asset = new Asset("EUR/USD", AssetType.Currency);
            IDataSource source = new DukascopyDataSource();
            FinancialDataBuffer buffer = new FinancialDataBuffer(10000);
            DukascopyOfflineProvider.Instance.AsyncGetHistory(
                asset,
                new DateTime(2000, 1, 1),
                new DateTime(2015, 1, 1),
                new DukascopyOfflineContext(@"C:\Users\user\EURUSD_Ticks_2010.01.01_2017.12.19.csv", SampleType.Quote,
                    TimeSpan.Zero),
                (Func<ISample, bool>) (sample =>
                {
                    //prices.Add(((IQuote)sample).Ask);
                    return true;
                }));
            IFinancialIndicator sma = new SMA(14);
            prices.ForEach(price => { sma.Update(price); });
            Assert.IsTrue(sma.Value > 0);
        }
    }
}