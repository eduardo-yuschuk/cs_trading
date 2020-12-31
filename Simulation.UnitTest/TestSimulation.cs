/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using Simulation.Shared;
using System;
using System.Collections.Generic;

namespace Simulation.UnitTest
{
    class TestSimulation : BaseSimulation
    {
        public TestSimulation(decimal? takeProfitPoints, decimal? stopLossPoints, bool isTrailingStop = false) : base(
            takeProfitPoints, stopLossPoints, isTrailingStop)
        {
        }

        public override string Description => throw new NotImplementedException();

        public override void OnBar_UserCode(IBarContext context)
        {
            var bar = context.Bar;
            var seriesValues = context.SeriesValues;
            var ema10 = seriesValues["EMA_SMALL"];
            var ema20 = seriesValues["EMA_BIG"];

            //Debug.WriteLine("{0}, bar: {1}, ema9: {2}, ema13: {3}", bar.DateTime, bar.Close, ema9.Value, ema13.Value);
            if (ema10.Value > ema20.Value)
            {
                // up trend
                if (LongPositions.Count == 0)
                {
                    CreatePosition(PositionSide.Long, bar.DateTime, bar.Close, 1);
                }
            }

            if (ema20.Value > ema10.Value)
            {
                // down trend
                if (ShortPositions.Count == 0)
                {
                    CreatePosition(PositionSide.Short, bar.DateTime, bar.Close, 1);
                }
            }
        }

        public override void RegisterIndicators(List<IBar> bars, IIndicatorsRegistry indicatorsRegistry)
        {
            throw new NotImplementedException();
        }
    }
}