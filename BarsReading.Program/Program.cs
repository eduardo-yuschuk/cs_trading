/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using SeriesReading.Descriptor.Quotes;
using System;
using System.Linq;

namespace BarsReading.Program
{
    internal class Program
    {
        private static void Main()
        {
            var path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == "EURUSD")
                .ProviderDescriptors.Single(x => x.Name == "Dukascopy")
                .Path;
            var reader = BarsReader.Create(TimeSpan.FromMinutes(1), path, new DateTime(2010, 1, 1),
                new DateTime(2010, 1, 31));
            while (reader.Next(out var dateTime, out var price))
            {
                Console.WriteLine("{0} -> {1}", dateTime, price);
            }

            Console.ReadLine();
        }
    }
}