/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringUtils;

namespace YahooStockQuote.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestGetPrice()
        {
            var price = YSQReader.GetPrice("MSFT");
            price = Cleaner.CleanWebResult(price);
            Assert.IsNotNull(price);
            Assert.IsTrue(float.Parse(price) > 0);
        }

        [TestMethod]
        public void TestMethod()
        {
            var prices = YSQReader.GetHistoricalPrices("MSFT", new DateTime(2000, 1, 1), new DateTime(2015, 1, 1));
            Assert.IsNotNull(prices);
        }
    }
}