/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace Simulation.Shared
{
    public interface IPosition
    {
        PositionSide Side { get; }
        DateTime OpenDateTime { get; }
        decimal OpenPrice { get; }
        DateTime CloseDateTime { get; }
        decimal ClosePrice { get; }
        decimal Earnings { get; }
        PositionStatus Status { get; }
        void VerifyTakeProfitAndStopLoss(DateTime dateTime, decimal price);
        void AdjustTrailingStopLoss(decimal price);
        void Close(DateTime dateTime, decimal price);
    }
}