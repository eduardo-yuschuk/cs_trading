/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using FinancialData.Shared;
using Simulation;
using Simulation.Shared;

namespace Forex.Research
{
    internal class TwoSmaSimulation : BaseSimulation
    {
        public TwoSmaSimulation(decimal? takeProfitPoints, decimal? stopLossPoints, bool isTrailingStop = false) : base(
            takeProfitPoints, stopLossPoints, isTrailingStop)
        {
        }

        public override string Description => throw new System.NotImplementedException();

        public override void OnBar_UserCode(IBarContext context)
        {
            var bar = context.Bar;
            var seriesValues = context.SeriesValues;
            var emaSmall = seriesValues["EMA_SMALL"];
            var emaBig = seriesValues["EMA_BIG"];

            //Debug.WriteLine("{0}, bar: {1}, ema9: {2}, ema13: {3}", bar.DateTime, bar.Close, ema9.Value, ema13.Value);
            if (emaSmall.Value > emaBig.Value)
            {
                // up trend
                if (LongPositions.Count == 0)
                {
                    ClosePositions();
                    CreatePosition(PositionSide.Long, bar.DateTime, bar.Close, 1);
                }
            }

            if (emaBig.Value > emaSmall.Value)
            {
                // down trend
                if (ShortPositions.Count == 0)
                {
                    ClosePositions();
                    CreatePosition(PositionSide.Short, bar.DateTime, bar.Close, 1);
                }
            }
        }

        public override void RegisterIndicators(List<IBar> bars, IIndicatorsRegistry indicatorsRegistry)
        {
            throw new System.NotImplementedException();
        }
    }
}