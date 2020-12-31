/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Linq;
using SeriesReading.Descriptor.Quotes;

namespace SeriesReading.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            //string rootFolder = @"C:\quotes\EURUSD\Dukascopy\";
            //DateTime begin = new DateTime(2003, 06, 01, 15, 31, 49);
            //DateTime end = new DateTime(2013, 06, 15, 18, 55, 16);
            //ISeriesReader reader = new SeriesReader(rootFolder, begin, end);
            //DateTime dateTime;
            //decimal ask, bid;
            //while (reader.Next(out dateTime, out ask, out bid)) {
            //  Console.WriteLine("{0} -> {1}/{2}", dateTime, ask, bid);
            //}

            string path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == "EURUSD")
                .ProviderDescriptors.Single(x => x.Name == "Dukascopy")
                .Path;
            SeriesReader reader = new SeriesReader(path);
            DateTime dateTime;
            decimal ask, bid;
            int lastMonth = -1;
            while (reader.Next(out dateTime, out ask, out bid))
            {
                if (dateTime.Month != lastMonth)
                {
                    Console.WriteLine("{0} -> {1}/{2}", dateTime, ask, bid);
                    lastMonth = dateTime.Month;
                }
            }

            Console.ReadLine();
        }
    }
}