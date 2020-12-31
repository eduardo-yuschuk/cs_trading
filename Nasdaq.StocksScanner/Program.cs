/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using Nasdaq.StocksProvider;
using System;
using System.Linq;
using YahooStockQuote.FinancialDataProvider;

namespace Nasdaq.StocksScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            var stocks = Provider.GetStocks();
            stocks.ForEach(stock =>
            {
                var symbol = stock.Symbol;
                var begin = new DateTime(2014, 12, 1);
                var end = new DateTime(2014, 12, 31);
                var asset = new Asset(symbol, AssetType.Stock);
                ISamplePackage samplePackage = new YSQProvider().GetHistory(asset, begin, end, null);
                IBarPackage barPackage = (IBarPackage) samplePackage;
                var samples = barPackage.Samples;
                int samplesCount = samples.Count;
                if (samplesCount > 0)
                {
                    var lastValue = samples.Last().Close;
                    Console.WriteLine("Stock: {0} - Samples: {1} - LastValue: {2}", symbol, samplesCount, lastValue);
                }
                else
                {
                    Console.WriteLine("Stock: {0} - Samples: {1}", symbol, samplesCount);
                }
            });
        }
    }
}