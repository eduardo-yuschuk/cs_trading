/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using BarsReading;
using SeriesReading.Descriptor.Quotes;
using System;
using System.Linq;

namespace WeekBasedAnalysis
{
    struct Sample
    {
        DateTime time;
        decimal price;
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == "EURUSD")
                .ProviderDescriptors.Single(x => x.Name == "Dukascopy")
                .Path;

            BarsReader reader = BarsReader.Create(TimeSpan.FromMinutes(1), path);

            DateTime dateTime;
            decimal price;

            var begin = new DateTime(2014, 1, 4);
            var end = new DateTime(2014, 1, 11);

            while (reader.Next(out dateTime, out price))
            {
                if (dateTime > begin && dateTime < end)
                {
                    Console.WriteLine("{0} -> {1}", dateTime, price);
                }
            }

            Console.ReadLine();
        }
    }
}