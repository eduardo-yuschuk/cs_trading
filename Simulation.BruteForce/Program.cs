/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using YahooStockQuote.FinancialDataProvider;
using TaLib.Extension;
using Simulation.Shared;
using Charts;
using System.Windows.Media;
using Charts.Common;

namespace Simulation.BruteForce
{
    class Program
    {
        static void Main(string[] args)
        {
            string symbol = YSQSymbol.YSQIndex.SNP;
            DateTime begin = new DateTime(2013, 1, 1);
            DateTime end = new DateTime(2014, 1, 1);
            ISamplePackage samplePackage =
                new YSQProvider().GetHistory(new Asset(symbol, AssetType.Index), begin, end, null);
            IBarPackage barPackage = (IBarPackage) samplePackage;
            List<IBar> bars = barPackage.Samples;
            // simulation
            ISimulation bestSimulation = default(ISimulation);
            TaResult bestSimulationSmallEma = default(TaResult);
            TaResult bestSimulationBigEma = default(TaResult);
            ChartPool.CreateChart();
            int configurationsCount = 0;
            DateTime simulationsBegin = DateTime.Now;
            for (int small = 2; small < 14; small += 2)
            {
                for (int big = 30; big < 80; big += 5)
                {
                    for (int take = 10; take < 50; take += 10)
                    {
                        for (int stop = 10; stop < 50; stop += 10)
                        {
                            configurationsCount++;
                            var simulation = SmaSimulation.CreateLongOnly(2, take, stop);
                            simulation.SimulationInfo = string
                                .Format("EMA_SMALL: {0}, EMA_BIG: {1}, TakeProfit: {2}, StopLoss: {3}",
                                    small, big, take, stop);
                            SimulationRunner simulationRunner = new SimulationRunner(bars, simulation);
                            var smallEma = bars.EMA(small);
                            var bigEma = bars.EMA(big);
                            simulationRunner.AddSerie("EMA_SMALL", smallEma, false);
                            simulationRunner.AddSerie("EMA_BIG", bigEma, false);
                            simulationRunner.Execute();
                            Console.WriteLine(simulation.GetReport());
                            if (bestSimulation == default(ISimulation) || simulation.Earnings > bestSimulation.Earnings)
                            {
                                bestSimulation = simulation;
                                bestSimulationSmallEma = smallEma;
                                bestSimulationBigEma = bigEma;
                            }

                            ShowSimulation(simulation, bars, smallEma, bigEma);
                            Console.WriteLine("Press any key to exit...");
                            Console.ReadKey(true);
                        }
                    }
                }
            }

            Console.WriteLine("{0} configurations tested in {1}", configurationsCount, DateTime.Now - simulationsBegin);
            Console.WriteLine("Best simulation");
            ShowSimulation(bestSimulation, bars, bestSimulationSmallEma, bestSimulationBigEma);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }

        static void ShowSimulation(ISimulation simulation, List<IBar> bars, TaResult smallEma, TaResult bigEma)
        {
            Console.WriteLine(simulation.GetReport());
            ChartPool.ClearSeries();
            ChartPool.AddSeries(
                new List<Series>
                {
                    new Series("Prices", ChartType.Lines, Colors.Black,
                        bars.Select(x => new Sample(x.DateTime, (double) x.Close))),
                    new Series("EMA Small", ChartType.Lines, Colors.Red,
                        smallEma.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                    new Series("EMA Big", ChartType.Lines, Colors.Blue,
                        bigEma.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                    new Series("Trades", ChartType.Trades, Colors.Purple,
                        simulation.ClosedPositions.Select(x => new Trade(x.OpenDateTime, x.CloseDateTime, x.Side,
                            (double) x.OpenPrice, (double) x.ClosePrice))),
                }
            );
        }
    }
}