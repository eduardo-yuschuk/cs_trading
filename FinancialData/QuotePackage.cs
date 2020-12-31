/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System.Collections.Generic;

namespace FinancialData
{
    public class QuotePackage : IQuotePackage
    {
        public SampleType SampleType
        {
            get { return SampleType.Quote; }
        }

        public List<IQuote> Samples { get; set; }
        public Asset Asset { get; set; }
        public IDataSource Source { get; set; }
    }
}