/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using BarsReading;
using FinancialIndicator;
using FinancialSeries.Shared;
using SeriesReading.Descriptor.Quotes;
using System;
using System.Linq;
using System.Text;

namespace IndicatorsReading.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == "EURUSD")
                .ProviderDescriptors.Single(x => x.Name == "Dukascopy")
                .Path;

            // creo un groupReader para leer los bars e indicadores de forma coordinada
            var groupReader = new DateTimeAndPriceGroupReader();

            // voy a consumir el barsReader a través del groupReader
            BarsReader barsReader = BarsReader.Create(TimeSpan.FromMinutes(1), path);
            groupReader.AddReader(barsReader);

            // también un indicador basado en minutos
            IndicatorsReader indicatorsReader = IndicatorsReader.Create(new RSI(25), TimeSpan.FromMinutes(1), path);
            groupReader.AddReader(indicatorsReader);
            //FinancialTimeSpans.Minutes.ForEach(timeFrame => {
            //  IndicatorsReader indicatorsReader = IndicatorsReader.Create(new RSI(25), timeFrame, path);
            //  groupReader.AddReader(indicatorsReader);
            //});

            DateTime groupDateTime;
            decimal[] barAndIndicatorsPrices;

            while (groupReader.Next(out groupDateTime, out barAndIndicatorsPrices))
            {
                StringBuilder sb = new StringBuilder();
                barAndIndicatorsPrices.ToList().ForEach(price =>
                {
                    if (sb.Length > 0) sb.Append(", ");
                    sb.AppendFormat("{0:0.000000}", price);
                });
                Console.WriteLine("{0} -> [{1}]", groupDateTime, sb.ToString());
            }

            Console.ReadLine();
        }
    }
}