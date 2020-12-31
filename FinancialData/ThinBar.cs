/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialData
{
    public class ThinBar
    {
        public long BarIndex { get; private set; }
        public TimeSpan TimeFrame { get; private set; }
        public DateTime DateTime { get; private set; }
        public decimal Close { get; private set; }

        public ThinBar(TimeSpan timeFrame, DateTime dateTime, decimal price)
        {
            TimeFrame = timeFrame;
            DateTime = dateTime;
            BarIndex = dateTime.Ticks / TimeFrame.Ticks;
            Close = price;
        }

        public override string ToString()
        {
            return string.Format("Bar of {0} index [{1}] => {2} -> {3:0.000000}", TimeFrame, BarIndex, DateTime, Close);
        }
    }
}