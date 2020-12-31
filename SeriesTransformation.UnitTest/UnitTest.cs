/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeriesTransformation.Shared;
using static QuotesConversion.QuotesConverter;

namespace SeriesTransformation.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestImportQuotes()
        {
            ISeriesConverter converter = new SeriesConverter();
            QuotesProcessor processor =
                new QuotesProcessor(@"C:\quotes\EURUSD\Dukascopy\", QuoteDateTimeAsNumberParser.Instance);
            DateTime begin = new DateTime(2010, 1, 1);
            DateTime end = new DateTime(2010, 7, 1);
            converter.ImportQuotes(@"C:\hfdata\Temp\EURUSD_DUKAS_TICKS.csv", processor, begin, end);
        }
    }
}