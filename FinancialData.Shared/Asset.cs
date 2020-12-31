/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

namespace FinancialData.Shared
{
    public class Asset
    {
        public string Name { get; set; }
        public AssetType Type { get; set; }

        public Asset(string name, AssetType type)
        {
            Name = name;
            Type = type;
        }
    }
}