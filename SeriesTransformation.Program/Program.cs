/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Globalization;
using SeriesTransformation.Shared;
using static QuotesConversion.QuotesConverter;

namespace SeriesTransformation.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            ISeriesConverter converter = new SeriesConverter();
            var dateTimeParser = new QuoteDateTimeAsTextParser("yyyy.MM.dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            QuotesProcessor processor = new QuotesProcessor(@"C:\quotes\EURUSD\Dukascopy\", dateTimeParser);
            DateTime begin = new DateTime(2000, 1, 1);
            DateTime end = new DateTime(2020, 1, 1);
            //converter.ImportQuotes(@"C:\quotes\EURUSD\Dukascopy\EURUSD_DUKAS_TICKS.csv", processor, begin, end);
            //converter.ImportQuotes(@"D:\100 ROOT\250 TRADING\100 DATA\EURUSD_Ticks_2010.01.01_2017.12.19.csv", processor, begin, end);
            //converter.ImportQuotes(@"D:\100 ROOT\250 TRADING\100 DATA\EURUSD_Ticks_2010.01.01_2017.12.19.csv",
            //    processor, begin, end);
            //converter.ImportQuotes(@"D:\100 ROOT\250 TRADING\100 DATA\EURUSD_Ticks_2018.01.01_2018.06.18.csv",
            converter.ImportQuotes(@"D:\100 ROOT\250 TRADING\100 DATA\EURUSD_Ticks_2010.01.01_2017.12.19.csv",
                processor, begin, end);
        }
    }
}