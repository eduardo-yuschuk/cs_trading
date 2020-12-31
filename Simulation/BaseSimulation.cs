/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using Simulation.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinancialData;

namespace Simulation
{
    public abstract class BaseSimulation : ISimulation
    {
        protected List<IPosition> _positions;
        protected List<IPosition> _closedPositions;
        private IBar _lastBar;
        public List<IInstantValue<decimal>> Balance { get; } = new List<IInstantValue<decimal>>();
        public List<IBar> Bars { get; } = new List<IBar>();
        public decimal? TakeProfitPoints { get; private set; }
        public decimal? StopLossPoints { get; private set; }
        public bool IsTrailingStop { get; private set; }

        public BaseSimulation(decimal? takeProfitPoints, decimal? stopLossPoints, bool isTrailingStop = false)
        {
            _positions = new List<IPosition>();
            _closedPositions = new List<IPosition>();
            TakeProfitPoints = takeProfitPoints;
            StopLossPoints = stopLossPoints;
            IsTrailingStop = isTrailingStop;
        }

        public List<IPosition> Positions => _positions.ToList();

        public List<IPosition> LongPositions => _positions.Where(x => x.Side == PositionSide.Long).ToList();

        public List<IPosition> ShortPositions => _positions.Where(x => x.Side == PositionSide.Short).ToList();

        public List<IPosition> ClosedPositions => _closedPositions.ToList();

        public void CreatePosition(PositionSide positionSide, DateTime dateTime, decimal price, int size) =>
            _positions.Add(new Position(positionSide, dateTime, price, size, TakeProfitPoints, StopLossPoints));

        public void ClosePositions() => Positions.ForEach(x => x.Close(_lastBar.DateTime, _lastBar.Close));

        public void OnBar(IBarContext context)
        {
            _lastBar = context.Bar;
            Bars.Add(_lastBar);
            Positions.ForEach(x => x.VerifyTakeProfitAndStopLoss(_lastBar.DateTime, _lastBar.Close));
            if (IsTrailingStop) Positions.ForEach(x => x.AdjustTrailingStopLoss(_lastBar.Close));
            PurgePositions();
            OnBar_UserCode(context);
            PurgePositions();
            Balance.Add(new InstantValue<decimal>(_lastBar.DateTime, _closedPositions.Sum(x => x.Earnings)));
        }

        private void PurgePositions()
        {
            var closedPositions = _positions.Where(x => x.Status == PositionStatus.Closed).ToList();
            closedPositions.ForEach(x => _positions.Remove(x));
            _closedPositions.AddRange(closedPositions);
        }

        public abstract void OnBar_UserCode(IBarContext context);

        public string GetReport()
        {
            var longClosedPositions = _closedPositions.Where(x => x.Side == PositionSide.Long).ToList();
            var shortClosedPositions = _closedPositions.Where(x => x.Side == PositionSide.Short).ToList();
            var sb = new StringBuilder();
            sb.AppendLine(SimulationInfo);
            sb.AppendLine($"Trades count: {_closedPositions.Count}");
            sb.AppendLine($"Long trades count: {longClosedPositions.Count}");
            sb.AppendLine($"Long trades balance: {longClosedPositions.Sum(x => x.Earnings)}");
            sb.AppendLine($"Short trades count: {shortClosedPositions.Count}");
            sb.AppendLine($"Short trades balance: {shortClosedPositions.Sum(x => x.Earnings)}");
            return sb.ToString();
        }

        public abstract void RegisterIndicators(List<IBar> bars, IIndicatorsRegistry indicatorsRegistry);

        public decimal Earnings {
            get { return _closedPositions.Sum(x => x.Earnings); }
        }

        public string SimulationInfo { get; set; }

        public abstract string Description { get; }
    }
}