/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace SeriesReading.Shared
{
    public interface ISeriesReader
    {
        bool Next(out DateTime dateTime, out decimal ask, out decimal bid);
    }
}