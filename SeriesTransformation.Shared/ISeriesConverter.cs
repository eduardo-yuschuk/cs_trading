/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace SeriesTransformation.Shared
{
    public interface ISeriesConverter
    {
        void ImportQuotes(string sourceFilePath, IQuotesProcessor quotesProcessor, DateTime begin, DateTime end);
    }
}