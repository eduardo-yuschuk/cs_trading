/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using FinancialData.Shared;
using Simulation.Shared;
using TaLib.Extension;

namespace Simulation.Strategies
{
    public class TwoSma : BaseSimulation
    {
        private readonly int _smallEmaPeriod;
        private readonly int _bigEmaPeriod;

        public TwoSma(decimal? takeProfitPoints, decimal? stopLossPoints/*, bool isTrailingStop*/, int smallEmaPeriod,
            int bigEmaPeriod)
            : base(takeProfitPoints, stopLossPoints/*, isTrailingStop*/)
        {
            _smallEmaPeriod = smallEmaPeriod;
            _bigEmaPeriod = bigEmaPeriod;
        }

        public override string Description => $"TP: {TakeProfitPoints}, SL: {StopLossPoints}, SmallEMA: {_smallEmaPeriod}, BigEMA: {_bigEmaPeriod}";

        public static List<ConfigurableArgument> GetArgumentsDefault()
        {
            return new List<ConfigurableArgument>
            {
                new ConfigurableArgument("takeProfitPoints", typeof(decimal?), 25M),
                new ConfigurableArgument("stopLossPoints", typeof(decimal?), 25M),
                //new ConfigurableArgument("isTrailingStop", typeof(bool), false),
                new ConfigurableArgument("smallEmaPeriod", typeof(int), 14),
                new ConfigurableArgument("bigEmaPeriod", typeof(int), 60),
            };
        }

        public override void OnBar_UserCode(IBarContext context)
        {
            var bar = context.Bar;
            var seriesValues = context.SeriesValues;
            var emaSmall = seriesValues["EMA_SMALL"];
            var emaBig = seriesValues["EMA_BIG"];
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
            indicatorsRegistry.AddSerie("EMA_SMALL", bars.EMA(_smallEmaPeriod), false);
            indicatorsRegistry.AddSerie("EMA_BIG", bars.EMA(_bigEmaPeriod), false);
        }
    }
}