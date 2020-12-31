/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

#define _VERBOSE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BarsBuilder.Shared;
using System.IO;
using FinancialData;
using FinancialData.Shared;

// ReSharper disable CommentTypo

namespace BarsBuilder
{
    public class BarsCreator : IBarsCreator
    {
        public Asset Asset { get; set; }
        public IDataSource Source { get; set; }
        public TimeSpan TimeFrame { get; private set; }
        private List<Bar> _bars = new List<Bar>();
        public int BarsCount { get; private set; }
        public int StoredBars { get; private set; }
        private Bar _lastBar;
        private long _timeFrameTicks;

        public BarsCreator(Asset asset, IDataSource source, TimeSpan timeFrame, string storagePath)
        {
            Asset = asset;
            Source = source;
            TimeFrame = timeFrame;
            _timeFrameTicks = TimeFrame.Ticks;
            StoragePath = storagePath;
            //if (storagePath == null)
            //  throw new Exception("storagePath can't be null.");
        }

        private void AddBar(Bar newBar)
        {
#if _VERBOSE
      if (BarsCount > 0)
        Console.WriteLine(_lastBar);
#endif
            _bars.Add(newBar);
            _lastBar = newBar;
            BarsCount++;
            BackupBars(1000000);
        }

        private void BackupBars(int barsToRemove, bool removeAll = false)
        {
            if (StoragePath == null) return;
            if (BarsCount <= barsToRemove && !removeAll) return;
            CreateFile();
            Console.WriteLine("Saving {0} bars to {1}", barsToRemove, StoragePath);
            for (var i = 0; i < barsToRemove; i++)
            {
                _writer.Write(_bars[i].GetBytes());
            }

            _bars.RemoveRange(0, barsToRemove);
            BarsCount -= barsToRemove;
            StoredBars += barsToRemove;
            if (!removeAll) return;
            _writer.Flush();
            _writer.Close();
        }

        private void UpdateLastBar(DateTime dateTime, decimal price)
        {
            _lastBar.Update(dateTime, price);
        }

        private void FillTheGap(long lastBarIndex, long newQuoteBarIndex, decimal priceToFillWith)
        {
            long suposedNextBarIndex = lastBarIndex + 1;
            if (newQuoteBarIndex != suposedNextBarIndex)
            {
                long barIndex = suposedNextBarIndex;
                while (barIndex < newQuoteBarIndex)
                {
                    DateTime barBegin = new DateTime(barIndex * TimeFrame.Ticks);
                    AddBar(new Bar(Asset, Source, TimeFrame, barBegin, priceToFillWith));
                    barIndex++;
                }
            }
        }

        public void AddQuote(DateTime newQuoteDateTime, decimal newQuotePrice)
        {
            if (BarsCount == 0)
            {
                AddBar(new Bar(Asset, Source, TimeFrame, newQuoteDateTime, newQuotePrice));
                return;
            }

            var lastBarIndex = _lastBar.BarIndex;
            var newQuoteBarIndex = newQuoteDateTime.Ticks / TimeFrame.Ticks;
            if (newQuoteBarIndex == lastBarIndex)
            {
                UpdateLastBar(newQuoteDateTime, newQuotePrice);
                return;
            }

            FillTheGap(lastBarIndex, newQuoteBarIndex, _lastBar.Close);
            // every bar starts with the close price of the previous one
            var begin = new DateTime(newQuoteBarIndex * TimeFrame.Ticks);
            if (begin != (_lastBar.DateTime + TimeSpan.FromTicks(TimeFrame.Ticks)))
            {
                Debugger.Break();
            }

            AddBar(new Bar(Asset, Source, TimeFrame, begin, _lastBar.Close));
            // agrego la info nueva
            UpdateLastBar(newQuoteDateTime, newQuotePrice);
        }

        //public void Save(string filePath)
        //{
        //    if (File.Exists(filePath)) File.Delete(filePath);
        //    using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
        //    {
        //        _bars.ForEach(bar => writer.Write(bar.Close/*.GetBytes()*/));
        //    }
        //}
        BinaryWriter _writer;
        public string StoragePath { get; private set; }
        public string FilePath { get; private set; }
        private long _firstQuoteBarIndex;

        private void CreateFile()
        {
            if (StoragePath == null) return;
            if (FilePath != null) return;
            if (BarsCount <= 0)
            {
                throw new Exception("Unable to create the filePath based on first bar information.");
            }

            var firstBar = _bars[0];
            _firstQuoteBarIndex = firstBar.DateTime.Ticks / _timeFrameTicks;
            FilePath = $@"{StoragePath}\BARS_TF_{_timeFrameTicks}__IX_{_firstQuoteBarIndex}.bin";
            if (File.Exists(FilePath)) File.Delete(FilePath);
            _writer?.Close();
            _writer = new BinaryWriter(File.OpenWrite(FilePath));
        }

        public void Finish()
        {
            BackupBars(BarsCount, true);
            VerifyConsistency();
        }

        private void VerifyConsistency()
        {
            if (_lastBar == null) return;
            // -1 porque la primera bar también fue almaceneda
            var theoreticalLastBarIndex = _firstQuoteBarIndex + StoredBars - 1;
            if (_lastBar.BarIndex != theoreticalLastBarIndex)
            {
                throw new Exception("Inconsistent bars creation.");
            }
        }

        public List<IBar> GetBars()
        {
            return _bars.Cast<IBar>().ToList();
        }
    }
}