/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using Simulation.Shared;
using System;
using System.Collections.Generic;

namespace Simulation.BruteForce
{
    enum Trend
    {
        Up,
        Down,
        Undefined
    }

    class SmaSimulation : BaseSimulation
    {
        private double _threshold;
        private bool _longEnabled;
        private bool _shortEnabled;
        private Trend _trend = Trend.Undefined;

        public override string Description => throw new NotImplementedException();

        public SmaSimulation(decimal threshold, bool longEnabled, bool shortEnabled, decimal? takeProfitPoints,
            decimal? stopLossPoints, bool isTrailingStop = false)
            : base(takeProfitPoints, stopLossPoints, isTrailingStop)
        {
            _threshold = (double) threshold;
            _longEnabled = longEnabled;
            _shortEnabled = shortEnabled;
        }

        public override void OnBar_UserCode(IBarContext context)
        {
            IBar bar = context.Bar;
            var seriesValues = context.SeriesValues;
            var smallEma = seriesValues["EMA_SMALL"];
            var bigEma = seriesValues["EMA_BIG"];
            TryToOpenLongPositions(bar, smallEma, bigEma);
            TryToOpenShortPositions(bar, smallEma, bigEma);
        }

        public override void RegisterIndicators(List<IBar> bars, IIndicatorsRegistry indicatorsRegistry)
        {
            throw new NotImplementedException();
        }

        private void TryToOpenLongPositions(IBar bar, IInstantValue<double> smallEma, IInstantValue<double> bigEma)
        {
            // up trend detected
            if (smallEma.Value > (bigEma.Value + _threshold))
            {
                // is trend change
                if (_trend != Trend.Up)
                {
                    _trend = Trend.Up;
                    if (_longEnabled && LongPositions.Count == 0)
                    {
                        CreatePosition(PositionSide.Long, bar.DateTime, bar.Close, 1);
                    }
                }
            }
        }

        private void TryToOpenShortPositions(IBar bar, IInstantValue<double> smallEma, IInstantValue<double> bigEma)
        {
            // down trend detected
            if (bigEma.Value > (smallEma.Value + _threshold))
            {
                // is trend change
                if (_trend != Trend.Down)
                {
                    _trend = Trend.Down;
                    if (_shortEnabled && ShortPositions.Count == 0)
                    {
                        CreatePosition(PositionSide.Short, bar.DateTime, bar.Close, 1);
                    }
                }
            }
        }

        public static SmaSimulation CreateBothSides(decimal threshold, decimal takeProfit, decimal stopLoss)
        {
            return new SmaSimulation(threshold, true, true, takeProfit, stopLoss, true);
        }

        public static SmaSimulation CreateLongOnly(decimal threshold, decimal takeProfit, decimal stopLoss)
        {
            return new SmaSimulation(threshold, true, false, takeProfit, stopLoss, true);
        }

        public static SmaSimulation CreateShortOnly(decimal threshold, decimal takeProfit, decimal stopLoss)
        {
            return new SmaSimulation(threshold, false, true, takeProfit, stopLoss, true);
        }
    }
}