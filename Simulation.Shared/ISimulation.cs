/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.Collections.Generic;

namespace Simulation.Shared
{
    public interface ISimulation
    {
        void RegisterIndicators(List<IBar> bars, IIndicatorsRegistry indicatorsRegistry);
        void OnBar(IBarContext context);
        void CreatePosition(PositionSide positionSide, DateTime dateTime, decimal price, int size);
        List<IPosition> Positions { get; }
        List<IPosition> ClosedPositions { get; }
        List<IPosition> LongPositions { get; }
        List<IPosition> ShortPositions { get; }
        string GetReport();
        decimal Earnings { get; }
        string SimulationInfo { get; set; }
        List<IInstantValue<decimal>> Balance { get; }
        List<IBar> Bars { get; }
        string Description { get; }
    }
}