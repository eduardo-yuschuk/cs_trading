/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using BarsReading;
using Charts;
using Charts.Common;
using SeriesReading;
using SeriesReading.Descriptor.Quotes;

namespace UserInterface
{
    public class DataHelper
    {
        public static void CreatePeriod(out DateTime begin, out DateTime end, DataSelection dataSelection)
        {
            var year = dataSelection.Year;
            var month = dataSelection.Month;
            var days = dataSelection.Days;
            begin = new DateTime(year, month, days.First(), 0, 0, 0);
            end = new DateTime(year, month, days.Last(), 0, 0, 0) + TimeSpan.FromDays(1);
        }

        public static void CreatePeriod(out DateTime begin, out DateTime end, ByYearDataSelection dataSelection)
        {
            var year = dataSelection.Year;
            begin = new DateTime(year, 1, 1, 0, 0, 0);
            end = new DateTime(year, 12, 31, 0, 0, 0) + TimeSpan.FromDays(1);
        }

        public static void CreateQuotesSeries(out DateTime begin, out DateTime end, out Series quotes,
            DataSelection dataSelection)
        {
            var rootFolder = $@"{dataSelection.RootPath}\{dataSelection.AssetFolder}\{dataSelection.ProviderFolder}\";
            CreatePeriod(out begin, out end, dataSelection);
            var reader = new SeriesReader(rootFolder, begin, end);
            var samples = new List<Sample>();
            while (reader.Next(out var dateTime, out var ask, out _))
            {
                samples.Add(new Sample(dateTime, (double)ask));
            }

            quotes = new Series("Quotes", ChartType.Lines, Colors.Black, samples);
        }

        public static void CreateBarsSeries(out DateTime begin, out DateTime end, out Series bars,
            DataSelection dataSelection)
        {
            var path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == dataSelection.Asset)
                .ProviderDescriptors.Single(x => x.Name == dataSelection.Provider)
                .Path;
            CreatePeriod(out begin, out end, dataSelection);
            var timeFrame = TimeSpan.FromMinutes(1);
            var reader = BarsReader.Create(timeFrame, path, begin, end);
            var samples = new List<Candle>();
            while (reader.Next(out var bar))
            {
                samples.Add(new Candle(bar.DateTime, bar.DateTime + timeFrame, (double)bar.Open, (double)bar.High,
                    (double)bar.Low, (double)bar.Close));
            }

            bars = new Series("Bars", ChartType.Bars, Colors.Transparent, samples);
        }
    }
}