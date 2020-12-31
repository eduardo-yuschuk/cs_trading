/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeriesReading.Shared;

namespace SeriesReading.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void ReadingQuotesTest()
        {
            string rootFolder = @"C:\quotes\EURUSD\Dukascopy\";
            DateTime begin = new DateTime(2004, 06, 01, 15, 31, 49);
            DateTime end = new DateTime(2004, 06, 15, 18, 55, 16);
            ISeriesReader reader = new SeriesReader(rootFolder, begin, end);
            DateTime dateTime;
            decimal ask, bid;
            while (reader.Next(out dateTime, out ask, out bid))
            {
                Assert.IsTrue(dateTime > begin);
                Assert.IsTrue(dateTime <= end);
                Assert.IsTrue(ask >= bid);
            }
        }
    }
}