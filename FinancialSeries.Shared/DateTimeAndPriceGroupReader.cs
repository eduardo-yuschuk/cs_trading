/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialSeries.Shared
{
    public class DateTimeAndPriceGroupReader : IDateTimeAndPricesReader
    {
        List<IDateTimeAndPriceReader> _readers = new List<IDateTimeAndPriceReader>();

        public void AddReader(IDateTimeAndPriceReader reader)
        {
            _readers.Add(reader);
        }

        struct Reading
        {
            public DateTime DateTime;
            public decimal Price;
            public bool Result;
        }

        public bool Next(out DateTime dateTime, out decimal[] prices)
        {
            var readings = _readers.Select(x =>
            {
                Reading reading = new Reading();
                reading.Result = x.Next(out reading.DateTime, out reading.Price);
                return reading;
            }).ToList();

            if (readings.Count == 0) throw new Exception("Empty reader.");
            if (readings.Exists(x => x.Result != readings[0].Result)) throw new Exception("Corrupted files.");
            if (readings.Exists(x => x.DateTime != readings[0].DateTime)) throw new Exception("Corrupted files.");
            if (readings[0].Result)
            {
                dateTime = readings[0].DateTime;
                prices = new decimal[readings.Count];
                Array.ConstrainedCopy(readings.Select(x => x.Price).ToArray(), 0, prices, 0, readings.Count);
            }
            else
            {
                dateTime = default(DateTime);
                prices = default(decimal[]);
            }

            return readings[0].Result;
        }
    }
}