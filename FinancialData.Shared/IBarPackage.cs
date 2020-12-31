/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialData.Shared
{
    public interface IBarPackage : ISamplePackage<IBar>
    {
        TimeSpan Period { get; set; }
        Asset Asset { get; }
        IDataSource Source { get; }
    }
}