/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using SeriesTransformation.Shared;
using System.IO;
using QuotesConversion;

namespace SeriesTransformation
{
    public class SeriesConverter : ISeriesConverter
    {
        public void ImportQuotes(string sourceFilePath, IQuotesProcessor quotesProcessor, DateTime begin, DateTime end)
        {
            using (StreamReader reader = File.OpenText(sourceFilePath))
            {
                string line;
                bool endOfSearchSpaceReached = false;
                line = reader.ReadLine();
                while (!endOfSearchSpaceReached && (line = reader.ReadLine()) != null)
                {
                    DateTime currentDateTime =
                        QuotesConverter.GetQuoteDateTimeFromString(line, quotesProcessor.QuoteDateTimeParser);
                    if (currentDateTime > end)
                    {
                        endOfSearchSpaceReached = true;
                    }
                    else if (currentDateTime >= begin)
                    {
                        quotesProcessor.StoreQuoteFromString(line);
                    }
                }
            }
        }
    }
}