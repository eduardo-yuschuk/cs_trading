/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace Nasdaq.StocksProvider
{
    public class NasdaqStock
    {
        public string Symbol { get; private set; }
        public string Name { get; private set; }
        public string LastSale { get; private set; }
        public string MarketCap { get; private set; }
        public string ADR_TSO { get; private set; }
        public string IPOyear { get; private set; }
        public string Sector { get; private set; }
        public string Industry { get; private set; }
        public string SummaryQuote { get; private set; }

        public NasdaqStock(string line)
        {
            //"Symbol","Name","LastSale","MarketCap","ADR TSO","IPOyear","Sector","industry","Summary Quote",
            var parts = line.Split(new char[] {'"', ','}, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            Symbol = parts[i++];
            Name = parts[i++];
            LastSale = parts[i++];
            MarketCap = parts[i++];
            ADR_TSO = parts[i++];
            IPOyear = parts[i++];
            Sector = parts[i++];
            Industry = parts[i++];
            SummaryQuote = parts[i++];
        }
    }
}