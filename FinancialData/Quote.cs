/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.Globalization;
using TimeUtils;

namespace FinancialData
{
    public class Quote : IQuote
    {
        private static CultureInfo _culture = CultureInfo.GetCultureInfo("en-US");
        public Asset Asset { get; set; }
        public IDataSource Source { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Ask { get; set; }
        public decimal AskSize { get; set; }
        public decimal Bid { get; set; }
        public decimal BidSize { get; set; }

        public SampleType SampleType
        {
            get { return SampleType.Quote; }
        }

        public static Quote From3PartsString(string line, Shared.Asset asset, IDataSource source)
        {
            var parts = line.Split(new char[] {','});
            return new Quote
            {
                Asset = asset,
                Source = source,
                DateTime = Instant.FromMillisAfterEpoch(long.Parse(parts[0])),
                Ask = source.ConvertPrice(decimal.Parse(parts[1], _culture)),
                AskSize = 0,
                Bid = source.ConvertPrice(decimal.Parse(parts[2], _culture)),
                BidSize = 0,
            };
        }

        public static Quote From5PartsString(string line, Shared.Asset asset, IDataSource source)
        {
            var parts = line.Split(new char[] {','});
            return new Quote
            {
                Asset = asset,
                Source = source,
                DateTime = Instant.FromMillisAfterEpoch(long.Parse(parts[0])),
                Ask = source.ConvertPrice(decimal.Parse(parts[1], _culture)),
                AskSize = source.ConvertPrice(decimal.Parse(parts[2], _culture)),
                Bid = source.ConvertPrice(decimal.Parse(parts[3], _culture)),
                BidSize = source.ConvertPrice(decimal.Parse(parts[4], _culture)),
            };
        }
    }
}