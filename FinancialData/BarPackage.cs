/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;
using System.Collections.Generic;

namespace FinancialData
{
    public class BarPackage : IBarPackage
    {
        public SampleType SampleType
        {
            get { return SampleType.Bar; }
        }

        public TimeSpan Period { get; set; }
        public Asset Asset { get; set; }
        public IDataSource Source { get; set; }
        public List<IBar> Samples { get; set; }
    }
}