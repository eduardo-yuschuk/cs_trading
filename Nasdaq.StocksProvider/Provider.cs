/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nasdaq.StocksProvider
{
    public class Provider
    {
        public static List<NasdaqStock> GetStocks()
        {
            var files = new List<string> {@"C:\stocks\AMEX.csv", @"C:\stocks\NASDAQ.csv", @"C:\stocks\NYSE.csv"};
            var lines = files.SelectMany(file =>
            {
                var _lines = File.ReadAllLines(file).ToList();
                _lines.RemoveAt(0);
                return _lines;
            });
            var stocks = lines.Select(line => new NasdaqStock(line));
            return stocks.ToList();
        }
    }
}