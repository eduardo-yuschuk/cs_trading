/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using static QuotesConversion.QuotesConverter;

namespace SeriesTransformation.Shared
{
    public interface IQuotesProcessor
    {
        IQuoteDateTimeParser QuoteDateTimeParser { get; }
        void StoreQuoteFromString(string text);
    }
}