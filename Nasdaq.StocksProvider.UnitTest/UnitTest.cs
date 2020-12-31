/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nasdaq.StocksProvider.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var stocks = Provider.GetStocks();
            Assert.IsNotNull(stocks);
            Assert.IsTrue(stocks.Count > 0);
        }
    }
}