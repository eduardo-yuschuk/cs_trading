/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;

namespace FinancialData.Shared
{
    [Serializable]
    public enum AssetType
    {
        Currency,
        CurrencyFuture,
        CurrencyForward,
        Stock,
        Index,
    }
}