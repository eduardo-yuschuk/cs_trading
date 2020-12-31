/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using Simulation.Shared;
using System;
using System.Collections.Generic;

namespace Simulation
{
    class BarContext : IBarContext
    {
        public BarContext(IBar bar, Dictionary<string, IInstantValue<double>> seriesValues)
        {
            Bar = bar;
            SeriesValues = seriesValues;
            foreach (var value in SeriesValues.Values)
            {
                if (bar.DateTime != value.DateTime)
                {
                    throw new Exception("Invalid DateTime.");
                }
            }
        }

        public IBar Bar { get; private set; }
        public Dictionary<string, IInstantValue<double>> SeriesValues { get; private set; }
    }
}