/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using FinancialData.Shared;

namespace TradingConfiguration.Shared
{
    [Serializable]
    public class AssetConfiguration
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public AssetType Type { get; set; }
    }
}