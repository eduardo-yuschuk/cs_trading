/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using BarsReading;
using Charts;
using SeriesReading.Descriptor.Quotes;
using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Charts.Common;
using TaLib.Extension;

namespace Forex.Research
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == "EURUSD")
                .ProviderDescriptors.Single(x => x.Name == "Dukascopy")
                .Path;
            BarsReader reader = BarsReader.Create(TimeSpan.FromMinutes(1), path);
            var bars = reader.ReadBetween(new DateTime(2008, 1, 1), new DateTime(2008, 2, 1));
            Console.WriteLine("{0} bars readed", bars.Count);

            TaResult ema14 = bars.EMA(14);
            TaResult ema60 = bars.EMA(60);
//#if _
            var simulation = new TwoSmaSimulation(10, 5, true);
            SimulationRunner simulationRunner = new SimulationRunner(bars, simulation);
            simulationRunner.AddSerie("EMA_SMALL", ema14, false);
            simulationRunner.AddSerie("EMA_BIG", ema60, false);
            simulationRunner.Execute();
            Console.WriteLine(simulation.GetReport());
//#endif
//#if _
            ChartPool.CreateChart();
            ChartPool.ClearSeries();
            ChartPool.AddSeries(
                new List<Series>
                {
                    new Series("Prices", ChartType.Lines, Colors.Black,
                        bars.Select(x => new Sample(x.DateTime, (double) x.Close))),
                    new Series("EMA_SMALL", ChartType.Lines, Colors.Red,
                        ema14.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                    new Series("EMA_BIG", ChartType.Lines, Colors.Blue,
                        ema60.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                }
            );
//#endif
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}