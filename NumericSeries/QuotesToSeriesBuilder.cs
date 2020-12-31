/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using SeriesReading;
using SeriesReading.Shared;
using SeriesTransformation;
using SeriesTransformation.Shared;
using System;
using System.Collections.Generic;
using static QuotesConversion.QuotesConverter;

namespace NumericSeries
{
    /// <summary>
    /// Construye una serie de valores en base a datos que pueden estar en los quotes ya importados o en un archivo de texto plano.
    /// </summary>
    public class QuotesToSeriesBuilder
    {
        private string _rootFolder;
        private string _rawQuotesPath;

        /// <summary>
        /// Construye una instancia de QuotesToSeriesBuilder.
        /// </summary>
        /// <param name="rootFolder">La carpeta raíz de los quotes importados (usualmente 'E:\quotes\EURUSD\Dukascopy\' o algo así).</param>
        /// <param name="rawQuotesPath">El archivo de texto plano para la carga de quotes alternativa.</param>
        public QuotesToSeriesBuilder(string rootFolder, string rawQuotesPath)
        {
            _rootFolder = rootFolder;
            _rawQuotesPath = rawQuotesPath;
        }

        public List<decimal> GetValues(DateTime begin, DateTime end)
        {
            var values = LoadValues(begin, end);
            if (values.Count == 0)
            {
                Console.WriteLine("Quotes not found, import process started.");
                ISeriesConverter converter = new SeriesConverter();
                QuotesProcessor processor = new QuotesProcessor(_rootFolder, QuoteDateTimeAsNumberParser.Instance);
                converter.ImportQuotes(_rawQuotesPath, processor, begin, end);
                Console.WriteLine("Loading imported prices.");
                values = LoadValues(begin, end);
            }

            return values;
        }

        private List<decimal> LoadValues(DateTime begin, DateTime end)
        {
            ISeriesReader reader = new SeriesReader(_rootFolder, begin, end);
            DateTime dateTime;
            decimal ask, bid;
            List<decimal> values = new List<decimal>();
            while (reader.Next(out dateTime, out ask, out bid))
            {
                values.Add(ask);
            }

            return values;
        }
    }
}