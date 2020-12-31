/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Linq;
using SeriesReading.Descriptor.Quotes;

namespace SeriesReading
{
    public class SeriesInformation
    {
        public SeriesInformation(string providerFolder)
        {
            ProviderDescriptor descriptor = new ProviderDescriptor(providerFolder);
            var filePaths = descriptor.YearDescriptors
                .SelectMany(y => y.MonthDescriptors
                    .SelectMany(m => m.DayDescriptors
                        .Select(d => d.Path)));
            DateTime dateTime;
            decimal ask, bid;
            SeriesReader.CreateReaderForSingleFile(filePaths.First()).Next(out dateTime, out ask, out bid);
            Begin = dateTime;
            var reader = SeriesReader.CreateReaderForSingleFile(filePaths.Last());
            while (reader.Next(out dateTime, out ask, out bid))
            {
                End = dateTime;
            }
        }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }
    }
}