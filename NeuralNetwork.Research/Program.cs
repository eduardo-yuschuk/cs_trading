// http://www.quaetrix.com/Build2013.html

// For 2013 Microsoft Build Conference attendees
// June 25-28, 2013
// San Francisco, CA
//
// This is source for a C# console application.
// To compile you can 1.) create a new Visual Studio
// C# console app project named BuildNeuralNetworkDemo
// then zap away the template code and replace with this code,
// or 2.) copy this code into notepad, save as NeuralNetworkProgram.cs
// on your local machine, launch the special VS command shell
// (it knows where the csc.exe compiler is), cd-navigate to
// the directory containing the .cs file, type 'csc.exe
// NeuralNetworkProgram.cs' and hit enter, and then after 
// the compiler creates NeuralNetworkProgram.exe, you can
// run from the command line.
//
// This is an enhanced neural network. It is fully-connected
// and feed-forward. The training algorithm is back-propagation
// with momentum and weight decay. The input data is normalized
// so training is quite fast.
//
// You can use this code however you wish subject to the usual disclaimers
// (use at your own risk, etc.)

using FinancialData.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using YahooStockQuote.FinancialDataProvider;
using TaLib.Extension;
using Charts;
using Simulation;
using Simulation.Shared;
using System.Windows.Media;
using Charts.Common;

namespace NeuralNetwork.Research
{
    class Program
    {
        static void TrainWithSampleData()
        {
            List<double[]> data = new List<double[]>();

            data.Add(new double[] {5.1, 3.5, 1.4, 0.2, 0, 0, 1});
            data.Add(new double[] {4.9, 3.0, 1.4, 0.2, 0, 0, 1});
            data.Add(new double[] {4.7, 3.2, 1.3, 0.2, 0, 0, 1});
            data.Add(new double[] {4.6, 3.1, 1.5, 0.2, 0, 0, 1});
            data.Add(new double[] {5.0, 3.6, 1.4, 0.2, 0, 0, 1});
            data.Add(new double[] {5.4, 3.9, 1.7, 0.4, 0, 0, 1});
            data.Add(new double[] {4.6, 3.4, 1.4, 0.3, 0, 0, 1});
            data.Add(new double[] {5.0, 3.4, 1.5, 0.2, 0, 0, 1});
            data.Add(new double[] {4.4, 2.9, 1.4, 0.2, 0, 0, 1});
            data.Add(new double[] {4.9, 3.1, 1.5, 0.1, 0, 0, 1});

            data.Add(new double[] {5.4, 3.7, 1.5, 0.2, 0, 0, 1});
            data.Add(new double[] {4.8, 3.4, 1.6, 0.2, 0, 0, 1});
            data.Add(new double[] {4.8, 3.0, 1.4, 0.1, 0, 0, 1});
            data.Add(new double[] {4.3, 3.0, 1.1, 0.1, 0, 0, 1});
            data.Add(new double[] {5.8, 4.0, 1.2, 0.2, 0, 0, 1});
            data.Add(new double[] {5.7, 4.4, 1.5, 0.4, 0, 0, 1});
            data.Add(new double[] {5.4, 3.9, 1.3, 0.4, 0, 0, 1});
            data.Add(new double[] {5.1, 3.5, 1.4, 0.3, 0, 0, 1});
            data.Add(new double[] {5.7, 3.8, 1.7, 0.3, 0, 0, 1});
            data.Add(new double[] {5.1, 3.8, 1.5, 0.3, 0, 0, 1});

            data.Add(new double[] {5.4, 3.4, 1.7, 0.2, 0, 0, 1});
            data.Add(new double[] {5.1, 3.7, 1.5, 0.4, 0, 0, 1});
            data.Add(new double[] {4.6, 3.6, 1.0, 0.2, 0, 0, 1});
            data.Add(new double[] {5.1, 3.3, 1.7, 0.5, 0, 0, 1});
            data.Add(new double[] {4.8, 3.4, 1.9, 0.2, 0, 0, 1});
            data.Add(new double[] {5.0, 3.0, 1.6, 0.2, 0, 0, 1});
            data.Add(new double[] {5.0, 3.4, 1.6, 0.4, 0, 0, 1});
            data.Add(new double[] {5.2, 3.5, 1.5, 0.2, 0, 0, 1});
            data.Add(new double[] {5.2, 3.4, 1.4, 0.2, 0, 0, 1});
            data.Add(new double[] {4.7, 3.2, 1.6, 0.2, 0, 0, 1});

            data.Add(new double[] {4.8, 3.1, 1.6, 0.2, 0, 0, 1});
            data.Add(new double[] {5.4, 3.4, 1.5, 0.4, 0, 0, 1});
            data.Add(new double[] {5.2, 4.1, 1.5, 0.1, 0, 0, 1});
            data.Add(new double[] {5.5, 4.2, 1.4, 0.2, 0, 0, 1});
            data.Add(new double[] {4.9, 3.1, 1.5, 0.1, 0, 0, 1});
            data.Add(new double[] {5.0, 3.2, 1.2, 0.2, 0, 0, 1});
            data.Add(new double[] {5.5, 3.5, 1.3, 0.2, 0, 0, 1});
            data.Add(new double[] {4.9, 3.1, 1.5, 0.1, 0, 0, 1});
            data.Add(new double[] {4.4, 3.0, 1.3, 0.2, 0, 0, 1});
            data.Add(new double[] {5.1, 3.4, 1.5, 0.2, 0, 0, 1});

            data.Add(new double[] {5.0, 3.5, 1.3, 0.3, 0, 0, 1});
            data.Add(new double[] {4.5, 2.3, 1.3, 0.3, 0, 0, 1});
            data.Add(new double[] {4.4, 3.2, 1.3, 0.2, 0, 0, 1});
            data.Add(new double[] {5.0, 3.5, 1.6, 0.6, 0, 0, 1});
            data.Add(new double[] {5.1, 3.8, 1.9, 0.4, 0, 0, 1});
            data.Add(new double[] {4.8, 3.0, 1.4, 0.3, 0, 0, 1});
            data.Add(new double[] {5.1, 3.8, 1.6, 0.2, 0, 0, 1});
            data.Add(new double[] {4.6, 3.2, 1.4, 0.2, 0, 0, 1});
            data.Add(new double[] {5.3, 3.7, 1.5, 0.2, 0, 0, 1});
            data.Add(new double[] {5.0, 3.3, 1.4, 0.2, 0, 0, 1});

            data.Add(new double[] {7.0, 3.2, 4.7, 1.4, 0, 1, 0});
            data.Add(new double[] {6.4, 3.2, 4.5, 1.5, 0, 1, 0});
            data.Add(new double[] {6.9, 3.1, 4.9, 1.5, 0, 1, 0});
            data.Add(new double[] {5.5, 2.3, 4.0, 1.3, 0, 1, 0});
            data.Add(new double[] {6.5, 2.8, 4.6, 1.5, 0, 1, 0});
            data.Add(new double[] {5.7, 2.8, 4.5, 1.3, 0, 1, 0});
            data.Add(new double[] {6.3, 3.3, 4.7, 1.6, 0, 1, 0});
            data.Add(new double[] {4.9, 2.4, 3.3, 1.0, 0, 1, 0});
            data.Add(new double[] {6.6, 2.9, 4.6, 1.3, 0, 1, 0});
            data.Add(new double[] {5.2, 2.7, 3.9, 1.4, 0, 1, 0});

            data.Add(new double[] {5.0, 2.0, 3.5, 1.0, 0, 1, 0});
            data.Add(new double[] {5.9, 3.0, 4.2, 1.5, 0, 1, 0});
            data.Add(new double[] {6.0, 2.2, 4.0, 1.0, 0, 1, 0});
            data.Add(new double[] {6.1, 2.9, 4.7, 1.4, 0, 1, 0});
            data.Add(new double[] {5.6, 2.9, 3.6, 1.3, 0, 1, 0});
            data.Add(new double[] {6.7, 3.1, 4.4, 1.4, 0, 1, 0});
            data.Add(new double[] {5.6, 3.0, 4.5, 1.5, 0, 1, 0});
            data.Add(new double[] {5.8, 2.7, 4.1, 1.0, 0, 1, 0});
            data.Add(new double[] {6.2, 2.2, 4.5, 1.5, 0, 1, 0});
            data.Add(new double[] {5.6, 2.5, 3.9, 1.1, 0, 1, 0});

            data.Add(new double[] {5.9, 3.2, 4.8, 1.8, 0, 1, 0});
            data.Add(new double[] {6.1, 2.8, 4.0, 1.3, 0, 1, 0});
            data.Add(new double[] {6.3, 2.5, 4.9, 1.5, 0, 1, 0});
            data.Add(new double[] {6.1, 2.8, 4.7, 1.2, 0, 1, 0});
            data.Add(new double[] {6.4, 2.9, 4.3, 1.3, 0, 1, 0});
            data.Add(new double[] {6.6, 3.0, 4.4, 1.4, 0, 1, 0});
            data.Add(new double[] {6.8, 2.8, 4.8, 1.4, 0, 1, 0});
            data.Add(new double[] {6.7, 3.0, 5.0, 1.7, 0, 1, 0});
            data.Add(new double[] {6.0, 2.9, 4.5, 1.5, 0, 1, 0});
            data.Add(new double[] {5.7, 2.6, 3.5, 1.0, 0, 1, 0});

            data.Add(new double[] {5.5, 2.4, 3.8, 1.1, 0, 1, 0});
            data.Add(new double[] {5.5, 2.4, 3.7, 1.0, 0, 1, 0});
            data.Add(new double[] {5.8, 2.7, 3.9, 1.2, 0, 1, 0});
            data.Add(new double[] {6.0, 2.7, 5.1, 1.6, 0, 1, 0});
            data.Add(new double[] {5.4, 3.0, 4.5, 1.5, 0, 1, 0});
            data.Add(new double[] {6.0, 3.4, 4.5, 1.6, 0, 1, 0});
            data.Add(new double[] {6.7, 3.1, 4.7, 1.5, 0, 1, 0});
            data.Add(new double[] {6.3, 2.3, 4.4, 1.3, 0, 1, 0});
            data.Add(new double[] {5.6, 3.0, 4.1, 1.3, 0, 1, 0});
            data.Add(new double[] {5.5, 2.5, 4.0, 1.3, 0, 1, 0});

            data.Add(new double[] {5.5, 2.6, 4.4, 1.2, 0, 1, 0});
            data.Add(new double[] {6.1, 3.0, 4.6, 1.4, 0, 1, 0});
            data.Add(new double[] {5.8, 2.6, 4.0, 1.2, 0, 1, 0});
            data.Add(new double[] {5.0, 2.3, 3.3, 1.0, 0, 1, 0});
            data.Add(new double[] {5.6, 2.7, 4.2, 1.3, 0, 1, 0});
            data.Add(new double[] {5.7, 3.0, 4.2, 1.2, 0, 1, 0});
            data.Add(new double[] {5.7, 2.9, 4.2, 1.3, 0, 1, 0});
            data.Add(new double[] {6.2, 2.9, 4.3, 1.3, 0, 1, 0});
            data.Add(new double[] {5.1, 2.5, 3.0, 1.1, 0, 1, 0});
            data.Add(new double[] {5.7, 2.8, 4.1, 1.3, 0, 1, 0});

            data.Add(new double[] {6.3, 3.3, 6.0, 2.5, 1, 0, 0});
            data.Add(new double[] {5.8, 2.7, 5.1, 1.9, 1, 0, 0});
            data.Add(new double[] {7.1, 3.0, 5.9, 2.1, 1, 0, 0});
            data.Add(new double[] {6.3, 2.9, 5.6, 1.8, 1, 0, 0});
            data.Add(new double[] {6.5, 3.0, 5.8, 2.2, 1, 0, 0});
            data.Add(new double[] {7.6, 3.0, 6.6, 2.1, 1, 0, 0});
            data.Add(new double[] {4.9, 2.5, 4.5, 1.7, 1, 0, 0});
            data.Add(new double[] {7.3, 2.9, 6.3, 1.8, 1, 0, 0});
            data.Add(new double[] {6.7, 2.5, 5.8, 1.8, 1, 0, 0});
            data.Add(new double[] {7.2, 3.6, 6.1, 2.5, 1, 0, 0});

            data.Add(new double[] {6.5, 3.2, 5.1, 2.0, 1, 0, 0});
            data.Add(new double[] {6.4, 2.7, 5.3, 1.9, 1, 0, 0});
            data.Add(new double[] {6.8, 3.0, 5.5, 2.1, 1, 0, 0});
            data.Add(new double[] {5.7, 2.5, 5.0, 2.0, 1, 0, 0});
            data.Add(new double[] {5.8, 2.8, 5.1, 2.4, 1, 0, 0});
            data.Add(new double[] {6.4, 3.2, 5.3, 2.3, 1, 0, 0});
            data.Add(new double[] {6.5, 3.0, 5.5, 1.8, 1, 0, 0});
            data.Add(new double[] {7.7, 3.8, 6.7, 2.2, 1, 0, 0});
            data.Add(new double[] {7.7, 2.6, 6.9, 2.3, 1, 0, 0});
            data.Add(new double[] {6.0, 2.2, 5.0, 1.5, 1, 0, 0});

            data.Add(new double[] {6.9, 3.2, 5.7, 2.3, 1, 0, 0});
            data.Add(new double[] {5.6, 2.8, 4.9, 2.0, 1, 0, 0});
            data.Add(new double[] {7.7, 2.8, 6.7, 2.0, 1, 0, 0});
            data.Add(new double[] {6.3, 2.7, 4.9, 1.8, 1, 0, 0});
            data.Add(new double[] {6.7, 3.3, 5.7, 2.1, 1, 0, 0});
            data.Add(new double[] {7.2, 3.2, 6.0, 1.8, 1, 0, 0});
            data.Add(new double[] {6.2, 2.8, 4.8, 1.8, 1, 0, 0});
            data.Add(new double[] {6.1, 3.0, 4.9, 1.8, 1, 0, 0});
            data.Add(new double[] {6.4, 2.8, 5.6, 2.1, 1, 0, 0});
            data.Add(new double[] {7.2, 3.0, 5.8, 1.6, 1, 0, 0});

            data.Add(new double[] {7.4, 2.8, 6.1, 1.9, 1, 0, 0});
            data.Add(new double[] {7.9, 3.8, 6.4, 2.0, 1, 0, 0});
            data.Add(new double[] {6.4, 2.8, 5.6, 2.2, 1, 0, 0});
            data.Add(new double[] {6.3, 2.8, 5.1, 1.5, 1, 0, 0});
            data.Add(new double[] {6.1, 2.6, 5.6, 1.4, 1, 0, 0});
            data.Add(new double[] {7.7, 3.0, 6.1, 2.3, 1, 0, 0});
            data.Add(new double[] {6.3, 3.4, 5.6, 2.4, 1, 0, 0});
            data.Add(new double[] {6.4, 3.1, 5.5, 1.8, 1, 0, 0});
            data.Add(new double[] {6.0, 3.0, 4.8, 1.8, 1, 0, 0});
            data.Add(new double[] {6.9, 3.1, 5.4, 2.1, 1, 0, 0});

            data.Add(new double[] {6.7, 3.1, 5.6, 2.4, 1, 0, 0});
            data.Add(new double[] {6.9, 3.1, 5.1, 2.3, 1, 0, 0});
            data.Add(new double[] {5.8, 2.7, 5.1, 1.9, 1, 0, 0});
            data.Add(new double[] {6.8, 3.2, 5.9, 2.3, 1, 0, 0});
            data.Add(new double[] {6.7, 3.3, 5.7, 2.5, 1, 0, 0});
            data.Add(new double[] {6.7, 3.0, 5.2, 2.3, 1, 0, 0});
            data.Add(new double[] {6.3, 2.5, 5.0, 1.9, 1, 0, 0});
            data.Add(new double[] {6.5, 3.0, 5.2, 2.0, 1, 0, 0});
            data.Add(new double[] {6.2, 3.4, 5.4, 2.3, 1, 0, 0});
            data.Add(new double[] {5.9, 3.0, 5.1, 1.8, 1, 0, 0});

            Trainer.Execute(data);
        }

        static void TrainWithIndexData()
        {
            List<double[]> data = new List<double[]>();

            var symbol = YSQSymbol.YSQIndex.SNP;
            var begin = new DateTime(2004, 1, 1);
            var end = new DateTime(2008, 1, 1);
            var samplePackage = new YSQProvider().GetHistory(new Asset(symbol, AssetType.Index), begin, end, null);
            var barPackage = (IBarPackage) samplePackage;
            var bars = barPackage.Samples;

            TaResult indicator0 = bars.SMA(7);
            TaResult indicator1 = bars.SMA(14);
            TaResult indicator2 = bars.SMA(24);
            TaResult indicator3 = bars.SMA(60);

            List<TaResult> indicatorsList = new List<TaResult> {indicator0, indicator1, indicator2, indicator3};

            // discard bars and indicators previous to valid series
            int firstValidSampleOnSeries = TaResult.GetFirstValidSample(indicatorsList);
            Console.WriteLine("First valid sample on series: {0}", firstValidSampleOnSeries);

            bars.RemoveRange(0, firstValidSampleOnSeries);
            TaResult.DiscardFirstSamples(indicatorsList, firstValidSampleOnSeries);
            Console.WriteLine("Invalid samples removed");

            // possible answers
            var stay = new double[] {1, 0, 0};
            var goLong = new double[] {0, 1, 0};
            var goShort = new double[] {0, 0, 1};

            var deltaPrices = bars.Select(x => x.Close - x.Open).ToList();
            var action = deltaPrices.Select(deltaPrice =>
            {
                var abs = Math.Abs(deltaPrice);
                if (abs < 5) return stay;
                if (deltaPrice < 0) return goShort;
                return goLong;
            }).ToList();

            Console.WriteLine("Times to stay: {0}", action.Count(x => x == stay));
            Console.WriteLine("Times to go long: {0}", action.Count(x => x == goLong));
            Console.WriteLine("Times to go short: {0}", action.Count(x => x == goShort));

            int samplesCount = bars.Count;

            for (int i = 0; i < samplesCount; i++)
            {
                var entry = new double[]
                {
                    indicator0.InstantValues[i].Value,
                    indicator1.InstantValues[i].Value,
                    indicator2.InstantValues[i].Value,
                    indicator3.InstantValues[i].Value,
                    action[i][0],
                    action[i][1],
                    action[i][2]
                };
                data.Add(entry);
            }

            NeuralNetwork neuralNetwork = Trainer.Execute(data);

            ChartPool.CreateChart();

            int takeProfitPoints = 15;
            int stopLossPoints = 10;
            decimal intensityThreshold = 0.0m;

            indicator0.Normalize();
            indicator1.Normalize();
            indicator2.Normalize();
            indicator3.Normalize();

            var simulation =
                new NNSimulation(neuralNetwork, intensityThreshold, takeProfitPoints, stopLossPoints, true);
            SimulationRunner simulationRunner = new SimulationRunner(bars, simulation);
            simulationRunner.AddSerie("INDICATOR_0", indicator0, false);
            simulationRunner.AddSerie("INDICATOR_1", indicator1, false);
            simulationRunner.AddSerie("INDICATOR_2", indicator2, false);
            simulationRunner.AddSerie("INDICATOR_3", indicator3, false);
            simulationRunner.Execute();
            ShowSimulation(simulation, bars, indicator0, indicator1, indicator2, indicator3);
        }

        static void ShowSimulation(ISimulation simulation, List<IBar> bars, TaResult indicator0, TaResult indicator1,
            TaResult indicator2, TaResult indicator3)
        {
            Console.WriteLine(simulation.GetReport());
            ChartPool.ClearSeries();
            ChartPool.AddSeries(
                new List<Series>
                {
                    new Series("Prices", ChartType.Lines, Colors.Black,
                        bars.Select(x => new Sample(x.DateTime, (double) x.Close))),
                    new Series("indicator0", ChartType.Lines, Colors.Blue,
                        indicator0.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                    new Series("indicator1", ChartType.Lines, Colors.DarkGreen,
                        indicator1.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                    new Series("indicator2", ChartType.Lines, Colors.DarkGreen,
                        indicator2.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                    new Series("indicator3", ChartType.Lines, Colors.DarkGreen,
                        indicator3.InstantValues.Select(x => new Sample(x.DateTime, x.Value))),
                    new Series("Trades", ChartType.Trades, Colors.Purple,
                        simulation.ClosedPositions.Select(x => new Trade(x.OpenDateTime, x.CloseDateTime, x.Side,
                            (double) x.OpenPrice, (double) x.ClosePrice))),
                }
            );
        }

        static void Main(string[] args)
        {
            TrainWithIndexData();
        }
    }
}