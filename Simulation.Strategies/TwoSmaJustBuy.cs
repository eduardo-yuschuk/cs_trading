using FinancialData.Shared;
using Simulation.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaLib.Extension;

namespace Simulation.Strategies
{
    public class TwoSmaJustBuy : BaseSimulation
    {
        private readonly int _smallEmaPeriod;
        private readonly int _bigEmaPeriod;
        private readonly int _smallRsiPeriod;
        private readonly int _bigRsiPeriod;

        public TwoSmaJustBuy(decimal? takeProfitPoints, decimal? stopLossPoints/*, bool isTrailingStop*/, int smallEmaPeriod, int bigEmaPeriod, int smallRsiPeriod, int bigRsiPeriod)
            : base(takeProfitPoints, stopLossPoints/*, isTrailingStop*/)
        {
            _smallEmaPeriod = smallEmaPeriod;
            _bigEmaPeriod = bigEmaPeriod;
            _smallRsiPeriod = smallRsiPeriod;
            _bigRsiPeriod = bigRsiPeriod;
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
                new ConfigurableArgument("smallRsiPeriod", typeof(int), 14),
                new ConfigurableArgument("bigRsiPeriod", typeof(int), 60),
            };
        }

        public override void OnBar_UserCode(IBarContext context)
        {
            var bar = context.Bar;
            var seriesValues = context.SeriesValues;
            var emaSmall = seriesValues["EMA_SMALL"];
            var emaBig = seriesValues["EMA_BIG"];
            var rsiSmall = seriesValues["RSI_SMALL"];
            var rsiBig = seriesValues["RSI_BIG"];
            if (emaSmall.Value > emaBig.Value)
            {
                // up trend
                if (LongPositions.Count == 0)
                {
                    CreatePosition(PositionSide.Long, bar.DateTime, bar.Close, 1);
                }
            }

            if (emaBig.Value > emaSmall.Value)
            {
                // down trend
                if (LongPositions.Count > 0)
                {
                    ClosePositions();
                }
            }
        }

        public override void RegisterIndicators(List<IBar> bars, IIndicatorsRegistry indicatorsRegistry)
        {
            indicatorsRegistry.AddSerie("EMA_SMALL", bars.EMA(_smallEmaPeriod), false);
            indicatorsRegistry.AddSerie("EMA_BIG", bars.EMA(_bigEmaPeriod), false);
            indicatorsRegistry.AddSerie("RSI_SMALL", bars.RSI(_smallEmaPeriod), true);
            indicatorsRegistry.AddSerie("RSI_BIG", bars.RSI(_bigEmaPeriod), true);
        }
    }
}