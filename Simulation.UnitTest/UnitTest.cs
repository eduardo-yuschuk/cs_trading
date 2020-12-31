/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YahooStockQuote.FinancialDataProvider;
using FinancialData.Shared;
using System.Collections.Generic;
using TaLib.Extension;
using System.Diagnostics;

namespace Simulation.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            // data
            string symbol = YSQSymbol.YSQIndex.SNP;
            DateTime begin = new DateTime(2000, 1, 1);
            DateTime end = new DateTime(2015, 1, 1);
            ISamplePackage samplePackage = new YSQProvider()
                .GetHistory(new Asset(symbol, AssetType.Index), begin, end, null);
            IBarPackage barPackage = (IBarPackage) samplePackage;
            List<IBar> bars = barPackage.Samples;
            // simulation
            var simulation = new TestSimulation(10, 5, true);
            SimulationRunner simulationRunner = new SimulationRunner(bars, simulation);
            simulationRunner.AddSerie("EMA_SMALL", bars.EMA(10), false);
            simulationRunner.AddSerie("EMA_BIG", bars.EMA(20), false);
            simulationRunner.Execute();
            Debug.WriteLine(simulation.GetReport());
        }
    }
}