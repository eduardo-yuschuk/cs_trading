/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;

namespace FinancialData.Shared
{
    public interface ISamplePackage
    {
        SampleType SampleType { get; }
    }

    public interface ISamplePackage<T> : ISamplePackage where T : ISample
    {
        List<T> Samples { get; }
    }
}