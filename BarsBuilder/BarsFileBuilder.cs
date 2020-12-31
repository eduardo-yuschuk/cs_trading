/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

#define _VERBOSE

using BarsBuilder.Shared;
using SeriesReading;
using SeriesReading.Descriptor.Quotes;
using System;
using System.Linq;

namespace BarsBuilder
{
    public class BarsFileBuilder
    {
        public static void Create(TimeSpan timeFrame)
        {
            IBarsCreator creator = new BarsCreator(null, null, timeFrame, @"C:\quotes\EURUSD\Dukascopy\");
            var path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == "EURUSD")
                .ProviderDescriptors.Single(x => x.Name == "Dukascopy")
                .Path;
            var reader = new SeriesReader(path);
#if _VERBOSE
            var lastMonth = -1;
#endif
            while (reader.Next(out var dateTime, out var ask, out _))
            {
                creator.AddQuote(dateTime, ask);
#if _VERBOSE
                if (dateTime.Month != lastMonth)
                {
                    Console.WriteLine("{0} -> {1}/{2} -> {3}", dateTime, ask, 0.0M, creator.BarsCount);
                    lastMonth = dateTime.Month;
                }
#endif
            }

            creator.Finish();
        }
    }
}