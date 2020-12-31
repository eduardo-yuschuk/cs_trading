/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using BarsReading;
using FinancialIndicator;
using FinancialSeriesUtils;
using SeriesReading.Descriptor.Quotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indicators.Automation
{
    public class Robot
    {
        public void Start()
        {
            FinancialTimeSpans.All.ForEach(timeFrame =>
            {
                string path = new SeriesDescriptor()
                    .InstrumentDescriptors.Single(x => x.Name == "EURUSD")
                    .ProviderDescriptors.Single(x => x.Name == "Dukascopy")
                    .Path;

                BarsReader reader = BarsReader.Create(timeFrame, path);

                if (reader != null)
                {
                    IndicatorsCreator creator = new IndicatorsCreator(timeFrame, path);
                    creator.AddIndicator(new RSI(15));
                    creator.AddIndicator(new RSI(20));
                    creator.AddIndicator(new RSI(25));

                    DateTime dateTime;
                    decimal price;
                    int lastMonth = -1;

                    while (reader.Next(out dateTime, out price))
                    {
                        creator.Update(dateTime, price);
                        if (dateTime.Month != lastMonth)
                        {
                            Console.WriteLine("{0} -> {1} -> {2}", dateTime, price, CreateString(creator.Values));
                            lastMonth = dateTime.Month;
                        }
                    }

                    creator.Finish();
                }
            });
        }

        private string CreateString(List<decimal> values)
        {
            StringBuilder sb = new StringBuilder();
            values.ForEach(value => sb.AppendFormat("{0:0.000000} ", value));
            return sb.ToString();
        }
    }
}