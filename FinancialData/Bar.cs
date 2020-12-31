/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.Collections.Generic;

namespace FinancialData
{
    public class Bar : IBar
    {
        public SampleType SampleType => SampleType.Bar;
        public Asset Asset { get; set; }
        public IDataSource Source { get; set; }
        public long BarIndex { get; private set; }
        public TimeSpan TimeFrame { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public decimal AdjClose { get; set; }
        public int SamplesCount { get; private set; }
        public DateTime LastDateTime { get; private set; }
        public static long BinarySize { get; } = sizeof(long) * 5;

        public Bar(Asset asset, IDataSource source, TimeSpan timeFrame, DateTime dateTime)
        {
            Asset = asset;
            Source = source;
            TimeFrame = timeFrame;
            BarIndex = dateTime.Ticks / TimeFrame.Ticks;
            LastDateTime = DateTime = new DateTime(TimeFrame.Ticks * BarIndex);
        }

        public Bar(Asset asset, IDataSource source, TimeSpan timeFrame, DateTime dateTime, decimal price)
            : this(asset, source, timeFrame, dateTime)
        {
            Open = High = Low = Close = price;
            SamplesCount++;
        }

        public Bar(Asset asset, IDataSource source, TimeSpan timeFrame, DateTime dateTime, decimal open, decimal high,
            decimal low, decimal close, long volume)
            : this(asset, source, timeFrame, dateTime)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            SamplesCount++;
        }

        public Bar(Asset asset, IDataSource source, TimeSpan timeFrame, DateTime dateTime, decimal open, decimal high,
            decimal low, decimal close, long volume, decimal adjClose)
            : this(asset, source, timeFrame, dateTime, open, high, low, close, volume)
        {
            AdjClose = adjClose;
        }

        public void Update(DateTime dateTime, decimal price)
        {
            if (dateTime < LastDateTime)
            {
                throw new Exception("Invalid DateTime for this Bar (mixed times).");
            }

            if (BarIndex != (dateTime.Ticks / TimeFrame.Ticks))
            {
                throw new Exception("Invalid DateTime for this Bar (out of bar sample).");
            }

            if (price > High) High = price;
            if (price < Low) Low = price;
            Close = price;
            LastDateTime = dateTime;
            SamplesCount++;
        }

        public byte[] GetBytes()
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(DateTime.Ticks));
            bytes.AddRange(BitConverter.GetBytes((long) (Open * 1000000)));
            bytes.AddRange(BitConverter.GetBytes((long) (High * 1000000)));
            bytes.AddRange(BitConverter.GetBytes((long) (Low * 1000000)));
            bytes.AddRange(BitConverter.GetBytes((long) (Close * 1000000)));
            return bytes.ToArray();
        }

        public override string ToString()
        {
            return
                $"Bar of {TimeFrame} index [{BarIndex}] => {DateTime}, samples {SamplesCount:000} -> {Open:0.000000} / {High:0.000000} / {Low:0.000000} / {Close:0.000000} - {Volume} - {AdjClose:0.000000}";
        }

        public static IBar FromBytes(TimeSpan timeFrame, byte[] bytes)
        {
            var index = 0;
            var dateTime = DateTime.FromBinary(BitConverter.ToInt64(bytes, index));
            index += sizeof(long);
            var open = BitConverter.ToInt64(bytes, index) / 1000000M;
            index += sizeof(long);
            var high = BitConverter.ToInt64(bytes, index) / 1000000M;
            index += sizeof(long);
            var low = BitConverter.ToInt64(bytes, index) / 1000000M;
            index += sizeof(long);
            var close = BitConverter.ToInt64(bytes, index) / 1000000M;
            const long volume = 0;
            var bar = new Bar(asset: null, source: null, timeFrame, dateTime, open, high, low, close, volume);
            return bar;
        }
    }
}