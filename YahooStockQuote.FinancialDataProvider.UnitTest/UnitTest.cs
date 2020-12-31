/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinancialData.Shared;

namespace YahooStockQuote.FinancialDataProvider.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestActualPrice()
        {
            ISample sample = new YSQProvider().GetPrice(new Asset("MSFT", AssetType.Stock));
            Assert.IsTrue(sample is IQuote);
            IQuote quote = (IQuote) sample;
            Assert.IsNotNull(quote.Asset);
            Assert.IsNotNull(quote.Source);
            Assert.IsNotNull(quote.DateTime);
            Assert.IsTrue(quote.Ask > 0);
        }

        [TestMethod]
        public void TestHistoricalPrices()
        {
            ISamplePackage samplePackage = new YSQProvider().GetHistory(new Asset("MSFT", AssetType.Stock),
                new DateTime(2014, 1, 1), new DateTime(2014, 7, 1), null);
            Assert.IsTrue(samplePackage is ISamplePackage<IBar>,
                "samplePackage no es instancia de ISamplePackage<IBar>");
            IBarPackage barPackage = (IBarPackage) samplePackage;
            Assert.IsNotNull(barPackage.Samples);
            Assert.IsTrue(barPackage.Samples.Count > 0);
            Assert.IsNotNull(barPackage.Asset);
            Assert.IsNotNull(barPackage.Source);
            Assert.IsNotNull(barPackage.Period);
        }
    }
}