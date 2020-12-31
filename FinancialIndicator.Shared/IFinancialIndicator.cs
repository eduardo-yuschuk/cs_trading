/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace FinancialIndicator.Shared
{
    public interface IFinancialIndicator
    {
        string Identifier { get; }
        void Update(decimal price);
        decimal Value { get; }
        bool HasValue { get; }
    }
}