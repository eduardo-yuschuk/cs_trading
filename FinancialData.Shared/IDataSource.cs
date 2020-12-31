/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace FinancialData.Shared
{
    public interface IDataSource
    {
        DataProvider Provider { get; }
        decimal ConvertPrice(decimal price);
    }
}