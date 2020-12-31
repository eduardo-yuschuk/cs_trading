/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialData.Shared
{
    public interface IProvisionContext
    {
        string Source { get; }
        SampleType SampleType { get; }
        TimeSpan Period { get; set; }
    }
}