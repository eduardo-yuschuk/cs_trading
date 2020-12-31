/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Simulation.Shared;
using System;

namespace Simulation
{
    class Position : IPosition
    {
        public PositionSide Side { get; private set; }
        public DateTime OpenDateTime { get; private set; }
        public decimal OpenPrice { get; private set; }
        public DateTime CloseDateTime { get; private set; }
        public decimal ClosePrice { get; private set; }
        public PositionStatus Status { get; private set; }
        public decimal? TakeProfitPoints { get; set; }
        public decimal? StopLossPoints { get; set; }
        public decimal? TakeProfitPrice { get; set; }
        public decimal? StopLossPrice { get; set; }
        public int Size { get; private set; }
        public CloseReason CloseReason { get; set; }

        public Position(PositionSide positionSide, DateTime dateTime, decimal price, int size,
            decimal? takeProfitPoints = null, decimal? stopLossPoints = null)
        {
            Side = positionSide;
            OpenDateTime = dateTime;
            OpenPrice = price;
            Size = size;
            TakeProfitPoints = takeProfitPoints;
            InitializeTakeProfit(OpenPrice);
            StopLossPoints = stopLossPoints;
            InitializeStopLoss(OpenPrice);
        }

        private void InitializeTakeProfit(decimal price)
        {
            if (TakeProfitPoints.HasValue)
            {
                switch (Side)
                {
                    case PositionSide.Long:
                        TakeProfitPrice = price + TakeProfitPoints;
                        break;
                    case PositionSide.Short:
                        TakeProfitPrice = price - TakeProfitPoints;
                        break;
                }
            }
        }

        private void InitializeStopLoss(decimal price)
        {
            if (StopLossPoints.HasValue)
            {
                switch (Side)
                {
                    case PositionSide.Long:
                        StopLossPrice = price - StopLossPoints;
                        break;
                    case PositionSide.Short:
                        StopLossPrice = price + StopLossPoints;
                        break;
                }
            }
        }

        public void Close(DateTime dateTime, decimal price)
        {
            if (Status == PositionStatus.Open)
            {
                CloseReason = CloseReason.External;
                Status = PositionStatus.Closed;
                CloseDateTime = dateTime;
                ClosePrice = price;
            }
        }

        public decimal Earnings
        {
            get { return Side == PositionSide.Long ? ClosePrice - OpenPrice : OpenPrice - ClosePrice; }
        }

        public void VerifyTakeProfitAndStopLoss(DateTime dateTime, decimal price)
        {
            if (Status == PositionStatus.Open)
            {
                switch (this.Side)
                {
                    case PositionSide.Long:
                    {
                        if (TakeProfitPoints.HasValue)
                        {
                            if (price >= TakeProfitPrice)
                            {
                                CloseReason = CloseReason.TakeProfit;
                                Close(dateTime, price);
                            }
                        }

                        if (StopLossPoints.HasValue)
                        {
                            if (price <= StopLossPrice)
                            {
                                CloseReason = CloseReason.StopLoss;
                                Close(dateTime, price);
                            }
                        }
                    }
                        break;
                    case PositionSide.Short:
                    {
                        if (TakeProfitPoints.HasValue)
                        {
                            if (price <= TakeProfitPrice)
                            {
                                CloseReason = CloseReason.TakeProfit;
                                Close(dateTime, price);
                            }
                        }

                        if (StopLossPoints.HasValue)
                        {
                            if (price >= StopLossPrice)
                            {
                                CloseReason = CloseReason.StopLoss;
                                Close(dateTime, price);
                            }
                        }
                    }
                        break;
                }
            }
        }

        public void AdjustTrailingStopLoss(decimal price)
        {
            if (Status == PositionStatus.Open)
            {
                if (StopLossPoints.HasValue)
                {
                    switch (this.Side)
                    {
                        case PositionSide.Long:
                        {
                            decimal newStopLossPrice = price - (decimal) StopLossPoints;
                            if (newStopLossPrice > StopLossPrice)
                            {
                                StopLossPrice = newStopLossPrice;
                            }
                        }
                            break;
                        case PositionSide.Short:
                        {
                            decimal newStopLossPrice = price + (decimal) StopLossPoints;
                            if (newStopLossPrice < StopLossPrice)
                            {
                                StopLossPrice = newStopLossPrice;
                            }
                        }
                            break;
                    }
                }
            }
        }
    }
}