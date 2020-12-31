/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeriesReading.Descriptor.Quotes;
using FinancialSeriesUtils;
using BarsReading;
using System.Linq;
using TaLib.Extension;
using System.Collections.Generic;
using FinancialData.Shared;

// ReSharper disable CommentTypo

namespace TaLib.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestSMA()
        {
            var path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == "EURUSD")
                .ProviderDescriptors.Single(x => x.Name == "Dukascopy")
                .Path;
            var timeFrame = FinancialTimeSpans.M1;
            var reader = BarsReader.Create(timeFrame, path);
            List<IBar> bars = reader.ReadAll();
            var smaValues = bars.SMA(60);
            Assert.AreEqual(bars.Count, smaValues.InstantValues.Count);
        }

        [TestMethod]
        public void TestBBands()
        {
            var path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == "EURUSD")
                .ProviderDescriptors.Single(x => x.Name == "Dukascopy")
                .Path;
            var timeFrame = FinancialTimeSpans.M1;
            var reader = BarsReader.Create(timeFrame, path);
            reader.ReadAll(out var bars);

            // corregir
            throw new InvalidOperationException();
            //var smaValues = prices.Bbands(15, 2, 2, TicTacTec.TA.Library.Core.MAType.Ema);

            //Assert.AreEqual(prices.Length, smaValues.Item1.Length);
            //Assert.AreEqual(prices.Length, smaValues.Item2.Length);
            //Assert.AreEqual(prices.Length, smaValues.Item3.Length);
        }
    }
}