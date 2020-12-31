/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System.Collections.Generic;

namespace Simulation.Shared
{
    public interface IBarContext
    {
        IBar Bar { get; }
        Dictionary<string, IInstantValue<double>> SeriesValues { get; }
    }
}