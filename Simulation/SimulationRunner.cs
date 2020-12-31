/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using Simulation.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaLib.Extension;

namespace Simulation
{
    public class SimulationRunner : IIndicatorsRegistry
    {
        public List<IBar> Bars { get; private set; }
        public ISimulation Simulation { get; private set; }
        public Dictionary<string, TaResult> Series { get; private set; } = new Dictionary<string, TaResult>();

        public SimulationRunner(List<IBar> bars, ISimulation simulation)
        {
            Bars = bars;
            Simulation = simulation;
            simulation.RegisterIndicators(bars, this);
        }

        public void AddSerie(string name, TaResult serie, bool normalize)
        {
            if (serie.Count != Bars.Count)
            {
                throw new Exception("Wrong samples qty.");
            }

            if(normalize)
            {
                serie = serie.ToNormalizedBetweenZeroAndOne();
            }

            Series[name] = serie;
        }

        private Dictionary<string, IInstantValue<double>> GetSeriesValues(int index)
        {
            var seriesValues = new Dictionary<string, IInstantValue<double>>();
            foreach (var serie in Series)
            {
                if (serie.Value.FirstValidSample > index)
                {
                    return null;
                }

                var serieKey = serie.Key;
                var serieValue = serie.Value;
                seriesValues[serieKey] = serieValue.InstantValues[index];
            }

            return seriesValues;
        }

        public void Execute()
        {
            var count = Bars.Count;
            for (var i = 0; i < count; i++)
            {
                var seriesValues = GetSeriesValues(i);
                if (seriesValues != null)
                {
                    Simulation.OnBar(new BarContext(Bars[i], seriesValues));
                }
            }
        }

        public class RealtimeProgress
        {
            public bool Started { get; set; }
            public decimal Progress { get; set; }
            public bool Finished { get; set; }
        }

        public RealtimeProgress ExecuteRealtime()
        {
            var progress = new RealtimeProgress();
            Task.Factory.StartNew(() =>
            {
                progress.Started = true;
                var count = Bars.Count;
                for (var i = 0; i < count; i++)
                {
                    var seriesValues = GetSeriesValues(i);
                    if (seriesValues != null)
                    {
                        Simulation.OnBar(new BarContext(Bars[i], seriesValues));
                    }
                    progress.Progress = i / (decimal)count;
                    Thread.Sleep(10);
                }
                progress.Finished = true;
            });

            return progress;
        }
    }
}