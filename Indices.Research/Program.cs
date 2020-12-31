/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.Collections.Generic;
using YahooStockQuote.FinancialDataProvider;

namespace Indices.Research
{
    class Program
    {
        static void Main(string[] args)
        {
            var symbol = YSQSymbol.YSQIndex.SNP;
            var begin = new DateTime(2000, 1, 1);
            var end = new DateTime(2015, 1, 1);
            var samplePackage = new YSQProvider().GetHistory(new Asset(symbol, AssetType.Index), begin, end, null);
            var barPackage = (IBarPackage) samplePackage;
            var bars = barPackage.Samples;
            //bars.ForEach(x => Console.WriteLine(x));

            IBar lastBar = null;
            List<double> deltas = new List<double>();
            foreach (var bar in bars)
            {
                if (lastBar != null)
                {
                    var delta = lastBar.Close - bar.Open;
                    deltas.Add((double) delta);
                }

                lastBar = bar;
            }

            // TODO solve this
            //var smoothDeltas = deltas.ToArray().SMA(6);

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey(true);
        }
    }
}