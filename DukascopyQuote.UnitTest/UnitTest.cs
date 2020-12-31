/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DukascopyQuote.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            int samples = 0;
            DukascopyOfflineReader.GetHistoricalPrices(
                @"C:\EURUSD_Ticks_2010.01.01_2017.12.19.csv",
                (Func<string, bool>) (line =>
                {
                    Console.WriteLine(line);
                    Assert.IsNotNull(line);
                    Assert.IsTrue(line.Length > 0);
                    return (++samples < 5);
                }));
            Assert.AreEqual(5, samples);
        }
    }
}