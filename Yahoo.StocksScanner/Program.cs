/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.Linq;
using Yahoo.YQL.StocksProvider;
using YahooStockQuote.FinancialDataProvider;

namespace Yahoo.StocksScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            var socks = Provider.GetStocks();
            socks.ForEach(stock =>
            {
                stock.Update();
                var symbol = stock.Symbol;
                var begin = new DateTime(2014, 12, 1);
                var end = new DateTime(2014, 12, 31);
                var asset = new Asset(symbol, AssetType.Stock);
                try
                {
                    ISamplePackage samplePackage = new YSQProvider().GetHistory(asset, begin, end, null);
                    IBarPackage barPackage = (IBarPackage) samplePackage;
                    stock.Update(barPackage);
                    var samples = barPackage.Samples;
                    int samplesCount = samples.Count;
                    if (samplesCount > 0)
                    {
                        var lastValue = samples.Last().Close;
                        Console.WriteLine("Stock: {0} - Samples: {1} - LastValue: {2}", symbol, samplesCount,
                            lastValue);
                    }
                    else
                    {
                        Console.WriteLine("Stock: {0} - Samples: {1}", symbol, samplesCount);
                    }
                }
                catch
                {
                    Console.WriteLine("Stock: {0} - NO DATA FOUND", symbol);
                }

                stock.Save();
            });
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}