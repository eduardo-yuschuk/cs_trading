/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialSeriesUtils
{
    public interface IDateTimeAndPriceReader
    {
        bool Next(out DateTime[] dateTime, out decimal[] price);
    }
}