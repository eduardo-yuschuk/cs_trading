/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using YahooStockQuote.FinancialDataProvider;
using TaLib.Extension;
using Charts;
using System.Windows.Media;
using Charts.Common;

namespace Indices.Elder.Research
{
    class Program
    {
        static void Main(string[] args)
        {
            string symbol = YSQSymbol.YSQIndex.SNP;
            DateTime begin = new DateTime(2003, 12, 1);
            DateTime end = new DateTime(2004, 2, 1);
            ISamplePackage samplePackage =
                new YSQProvider().GetHistory(new Asset(symbol, AssetType.Index), begin, end, null);
            IBarPackage barPackage = (IBarPackage) samplePackage;
            List<IBar> bars = barPackage.Samples;
            TaResult ema13 = bars.EMA(13);
            BullPower bullPower = new BullPower(bars, 13);

            //var sampleValues = new List<IInstantValue<double>>();
            //sampleValues.Add(new InstantValue<double>(new DateTime(2004, 1, 1), 1.0));
            //sampleValues.Add(new InstantValue<double>(new DateTime(2004, 1, 2), -1.0));
            //sampleValues.Add(new InstantValue<double>(new DateTime(2004, 1, 3), 1.0));
            //sampleValues.Add(new InstantValue<double>(new DateTime(2004, 1, 4), -1.0));

            ChartPool.CreateChart();
            ChartPool.ClearSeries();
            ChartPool.AddSeries(
                new List<Series>
                {
                    new Series("Prices", ChartType.Lines, Colors.Black,
                        bars.Select(x => new Sample(x.DateTime, (double) x.Close))),
                    new Series("EMA(13)", ChartType.Lines, Colors.Red,
                        ema13.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                    new Series("BullPower(13)", ChartType.Columns, Colors.Blue,
                        bullPower.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                }
            );

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}