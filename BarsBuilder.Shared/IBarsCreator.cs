/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.Collections.Generic;

namespace BarsBuilder.Shared
{
    public interface IBarsCreator
    {
        void AddQuote(DateTime dateTime, decimal price);

        int BarsCount { get; }

        //void Save(string filePath);

        string StoragePath { get; }

        void Finish();

        List<IBar> GetBars();
    }
}