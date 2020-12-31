/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using FinancialData.Shared;

namespace TradingConfiguration.Shared
{
    [Serializable]
    public class AssetsConfiguration
    {
        public List<AssetConfiguration> Assets { get; set; }

        public AssetsConfiguration()
        {
            Assets = new List<AssetConfiguration>();
        }

        public List<AssetType> AssetsTypes
        {
            get { return Assets.Select(x => x.Type).Distinct().ToList(); }
        }
    }
}