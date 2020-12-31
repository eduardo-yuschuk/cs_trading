/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialIndicator.Shared;
using FinancialSeries.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IndicatorsReading
{
    public class IndicatorsReader : IDateTimeAndPriceReader
    {
        private IFinancialIndicator _indicator;
        public TimeSpan TimeFrame { get; private set; }
        private long _timeFrameTicks;
        public string StoragePath { get; private set; }
        public string FilePath { get; private set; }
        private Queue<IndicatorFileInfo> _indicatorFileInfos = new Queue<IndicatorFileInfo>();

        public static IndicatorsReader Create(IFinancialIndicator indicator, TimeSpan timeFrame, string storagePath)
        {
            try
            {
                return new IndicatorsReader(indicator, timeFrame, storagePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private IndicatorsReader(IFinancialIndicator indicator, TimeSpan timeFrame, string storagePath)
        {
            _indicator = indicator;
            TimeFrame = timeFrame;
            _timeFrameTicks = TimeFrame.Ticks;
            StoragePath = storagePath;
            if (storagePath == null)
                throw new Exception("storagePath can't be null.");
            MapFiles();
        }

        private void MapFiles()
        {
            if (FilePath == null)
            {
                string pattern = string.Format(@"{0}_TF_{1}__IX_*.bin", _indicator.Identifier, _timeFrameTicks);
                var files = Directory.EnumerateFiles(StoragePath, pattern, SearchOption.TopDirectoryOnly);
                files
                    .Select(filePath => new IndicatorFileInfo(filePath, TimeFrame))
                    .OrderBy(indicatorFileInfo => indicatorFileInfo.FirstBarDateTime)
                    .ToList()
                    .ForEach(indicatorFileInfo => _indicatorFileInfos.Enqueue(indicatorFileInfo));

                if (_indicatorFileInfos.Count > 1)
                    throw new Exception("Many files case not supported yet (debug and test).");

                if (_indicatorFileInfos.Count == 0)
                    throw new Exception(string.Format("Indicator files to be readed ({0}) not found.", pattern));
            }
        }

        private BinaryReader _reader;
        private long _consumedBars = 0;

        private IndicatorFileInfo _currentIndicatorFileInfo;

        private IndicatorFileInfo NextIndicatorFileInfo()
        {
            _currentIndicatorFileInfo = _indicatorFileInfos.Dequeue();
            return _currentIndicatorFileInfo;
        }

        public bool Next(out DateTime dateTime, out decimal price)
        {
            if (!GetPriceFromBinaryReader(_reader, out dateTime, out price))
            {
                if (_indicatorFileInfos.Count == 0)
                {
                    dateTime = default(DateTime);
                    price = default(decimal);
                    return false;
                }

                if (_reader != null) _reader.Close();
                string path = NextIndicatorFileInfo().FilePath;
                while (!File.Exists(path))
                {
                    if (_indicatorFileInfos.Count > 0)
                    {
                        path = NextIndicatorFileInfo().FilePath;
                    }
                    else
                    {
                        dateTime = default(DateTime);
                        price = default(decimal);
                        return false;
                    }
                }

                _reader = new BinaryReader(File.OpenRead(path));
                if (!GetPriceFromBinaryReader(_reader, out dateTime, out price))
                {
                    throw new Exception("Empty file found.");
                }
            }

            return true;
        }

        public bool GetPriceFromBinaryReader(BinaryReader reader, out DateTime dateTime, out decimal price)
        {
            if (reader == null)
            {
                dateTime = default(DateTime);
                price = default(decimal);
                return false;
            }

            try
            {
                price = reader.ReadDecimal();
                dateTime = new DateTime(_currentIndicatorFileInfo.TimeFrame.Ticks *
                                        (_currentIndicatorFileInfo.FirstQuoteBarIndex + _consumedBars++));
                return true;
            }
            catch (Exception)
            {
                dateTime = default(DateTime);
                price = default(decimal);
                return false;
            }
        }
    }
}