/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingConfiguration.Shared;
using FinancialData.Shared;

namespace TradingConfiguration.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var configuration = Configuration.Instance;
            var assetsConfiguration = configuration.AssetsConfiguration;
            var assets = assetsConfiguration.Assets;
            assets.Clear();
            assets.Add(new AssetConfiguration {Name = "EUR/USD", ShortName = "EURUSD", Type = AssetType.Currency});
            assets.Add(new AssetConfiguration {Name = "GBP/USD", ShortName = "GBPUSD", Type = AssetType.Currency});
            configuration.Save();
        }
    }
}