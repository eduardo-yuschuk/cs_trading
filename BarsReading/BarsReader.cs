/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using BarsBuilder;
using FinancialData.Shared;
using FinancialSeries.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialData;

// ReSharper disable CommentTypo

namespace BarsReading
{
    public class BarsReader : IDateTimeAndPriceReader
    {
        public TimeSpan TimeFrame { get; private set; }

        //private List<ThinBar> _bars = new List<ThinBar>();
        //public int BarsCount { get; private set; }
        //public int StoredBars { get; private set; }
        public string StoragePath { get; private set; }
        public DateTime? Begin { get; private set; }

        public DateTime? End { get; private set; }

        //public string FilePath { get; private set; }
        private readonly long _timeFrameTicks;
        private readonly Queue<BarFileInfo> _barFileInfos = new Queue<BarFileInfo>();

        public static BarsReader Create(TimeSpan timeFrame, string storagePath)
        {
            try
            {
                return TryToCreate(timeFrame, storagePath);
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
                BarsFileBuilder.Create(timeFrame);
                try
                {
                    return TryToCreate(timeFrame, storagePath);
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                    return null;
                }
            }
        }

        public static BarsReader Create(TimeSpan timeFrame, string storagePath, DateTime begin, DateTime end)
        {
            try
            {
                return TryToCreate(timeFrame, storagePath, begin, end);
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
                BarsFileBuilder.Create(timeFrame);
                try
                {
                    return TryToCreate(timeFrame, storagePath);
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                    return null;
                }
            }
        }

        private static BarsReader TryToCreate(TimeSpan timeFrame, string storagePath)
        {
            return new BarsReader(timeFrame, storagePath);
        }

        private static BarsReader TryToCreate(TimeSpan timeFrame, string storagePath, DateTime begin, DateTime end)
        {
            return new BarsReader(timeFrame, storagePath, begin, end);
        }

        private BarsReader(TimeSpan timeFrame, string storagePath)
        {
            TimeFrame = timeFrame;
            _timeFrameTicks = TimeFrame.Ticks;
            StoragePath = storagePath;
            if (storagePath == null)
            {
                throw new Exception("storagePath can't be null.");
            }

            MapFiles();
        }

        private BarsReader(TimeSpan timeFrame, string storagePath, DateTime begin, DateTime end)
        {
            TimeFrame = timeFrame;
            _timeFrameTicks = TimeFrame.Ticks;
            StoragePath = storagePath;
            Begin = begin;
            End = end;
            if (storagePath == null)
            {
                throw new Exception("storagePath can't be null.");
            }

            MapFiles();
        }

        private void MapFiles()
        {
            //if (FilePath != null) return;
            var pattern = $@"BARS_TF_{_timeFrameTicks}__IX_*.bin";
            var files = Directory.EnumerateFiles(StoragePath, pattern, SearchOption.TopDirectoryOnly);
            files
                .Select(filePath => new BarFileInfo(filePath, TimeFrame))
                .OrderBy(barFileInfo => barFileInfo.FirstBarDateTime)
                .ToList()
                .ForEach(barFileInfo => _barFileInfos.Enqueue(barFileInfo));
            if (_barFileInfos.Count > 1)
                throw new Exception("Many files case not supported yet (debug and test).");

            if (_barFileInfos.Count == 0)
                throw new Exception($"Bar files to be read ({pattern}) not found.");
        }

        private BinaryReader _reader;
        private long _consumedBars;
        private BarFileInfo _currentBarFileInfo;

        private BarFileInfo NextBarFileInfo()
        {
            _currentBarFileInfo = _barFileInfos.Dequeue();
            return _currentBarFileInfo;
        }

        public bool Next(out IBar bar)
        {
            if (GetPriceFromBinaryReader(_reader, TimeFrame, out bar))
            {
                // si el dateTime en mayor al End, retornamos false para que aborte la iteración.
                return !End.HasValue || !(bar.DateTime >= End);
            }

            if (_barFileInfos.Count == 0)
            {
                bar = null;
                return false;
            }

            _reader?.Close();
            var path = NextBarFileInfo().FilePath;
            while (!File.Exists(path))
            {
                if (_barFileInfos.Count > 0)
                {
                    path = NextBarFileInfo().FilePath;
                }
                else
                {
                    bar = null;
                    return false;
                }
            }

            _reader = new BinaryReader(File.OpenRead(path));
            SearchBeginning();
            if (!GetPriceFromBinaryReader(_reader, TimeFrame, out bar))
            {
                throw new Exception("Empty file found.");
            }

            return true;
        }

        private void SearchBeginning()
        {
            if (!Begin.HasValue) return;
            var begin = Begin.Value;
            if (_currentBarFileInfo.FirstBarDateTime > begin) return;
            var delta = begin - _currentBarFileInfo.FirstBarDateTime;
            var samplesToJump = delta.Ticks / _currentBarFileInfo.TimeFrame.Ticks;
            if (samplesToJump > _currentBarFileInfo.SamplesInFile)
            {
                throw new Exception("Sample not found in bars file.");
            }

            _reader.BaseStream.Seek(samplesToJump * Bar.BinarySize, SeekOrigin.Begin);
            _consumedBars += samplesToJump;
        }

        public static bool GetPriceFromBinaryReader(BinaryReader reader, TimeSpan timeFrame, out IBar bar)
        {
            if (reader == null)
            {
                bar = null;
                return false;
            }

            try
            {
                bar = Bar.FromBytes(timeFrame, reader.ReadBytes((int) Bar.BinarySize));
                return true;
            }
            catch (Exception)
            {
                bar = null;
                return false;
            }
        }

        public int ReadAll(out IBar[] bars)
        {
            var lBars = new List<IBar>();
            while (Next(out var bar))
            {
                lBars.Add(bar);
            }

            bars = lBars.ToArray();
            return bars.Length;
        }

        public List<IBar> ReadAll()
        {
            ReadAll(out var bars);
            return bars.ToList();
        }

        public List<IBar> ReadBetween(DateTime begin, DateTime end)
        {
            var beforeFirstValid = begin.AddTicks(-1);
            var lastValid = end.AddTicks(-1);
            var bars = new List<IBar>();
            while (Next(out var bar))
            {
                if (bar.DateTime > lastValid)
                {
                    break;
                }

                if (bar.DateTime > beforeFirstValid)
                {
                    bars.Add(bar);
                }
            }

            return bars;
        }

        public static IBar ReadBarFromFile(TimeSpan timeFrame, string filePath)
        {
            var reader = new BinaryReader(File.OpenRead(filePath));
            if (!GetPriceFromBinaryReader(reader, timeFrame, out var bar))
            {
                throw new Exception("Empty file found.");
            }

            return bar;
        }

        public bool Next(out DateTime dateTime, out decimal price)
        {
            var result = Next(out var bar);
            dateTime = bar.DateTime;
            price = bar.Close;
            return result;
        }
    }
}