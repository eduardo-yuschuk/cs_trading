/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinancialData.Shared;

namespace DukascopyQuote.FinancialDataProvider.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestActualPrice()
        {
            //ISample sample = new DukascopyOfflineProvider().GetPrice("EUR/USD");
        }

        [TestMethod]
        public void TestHistoricalPrices()
        {
            ISamplePackage samplePackage = DukascopyOfflineProvider.Instance
                .GetHistory(
                    new Asset("EUR/USD", AssetType.Currency),
                    new DateTime(2014, 1, 1),
                    new DateTime(2014, 1, 31),
                    new DukascopyOfflineContext(
                        @"C:\EURUSD_Ticks_2010.01.01_2017.12.19.csv",
                        SampleType.Bar,
                        TimeSpan.FromMinutes(1)));
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