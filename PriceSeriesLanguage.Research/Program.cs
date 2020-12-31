/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using NumericSeries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceSeriesLanguage.Research
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime begin = new DateTime(2012, 1, 1);
            DateTime end = new DateTime(2012, 1, 2);

            var builder = new QuotesToSeriesBuilder(@"C:\quotes\EURUSD\Dukascopy\",
                @"C:\Users\user\EURUSD_Ticks_2010.01.01_2017.12.19.csv");
            var values = builder.GetValues(begin, end);
            Console.WriteLine("{0} values readed", values.Count);

            // primera capa
            var prevValue = values.First();
            values.RemoveAt(0);
            List<PriceVariation> firstLevel = new List<PriceVariation>();
            foreach (var value in values)
            {
                firstLevel.Add(new PriceVariation {Value1 = prevValue, Value2 = value});
            }

            var differentVariations = firstLevel
                .Select(x => x.Delta)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            Console.WriteLine("{0} variations found", differentVariations.Count);
            foreach (var variation in differentVariations)
            {
                Console.WriteLine(variation);
            }

            Console.ReadKey(true);
        }
    }

    class PriceVariation
    {
        public decimal Value1 { get; internal set; }
        public decimal Value2 { get; internal set; }

        public decimal Delta
        {
            get { return Value2 - Value1; }
        }
    }
}