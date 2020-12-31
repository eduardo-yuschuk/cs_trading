/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

//	based on ystockquote
//	original copyright

//  Copyright (c) 2007-2008, Corey Goldberg (corey@goldb.org)
//
//  license: GNU LGPL
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.

using System;
using System.Collections.Generic;
using System.Linq;
using WebReader;

namespace YahooStockQuote
{
    public class YSQReader
    {
        private static string Request(string symbol, string stat)
        {
            var url = $"http://finance.yahoo.com/d/quotes.csv?s={symbol}&f={stat}";
            return Reader.ReadString(url);
        }

        public static Dictionary<string, string> GetAll(string symbol)
        {
            var values = Request(symbol, "l1c1va2xj1b4j4dyekjm3m4rr5p5p6s7").Split(new char[] {','});
            var data = new Dictionary<string, string>
            {
                ["price"] = values[0],
                ["change"] = values[1],
                ["volume"] = values[2],
                ["avg_daily_volume"] = values[3],
                ["stock_exchange"] = values[4],
                ["market_cap"] = values[5],
                ["book_value"] = values[6],
                ["ebitda"] = values[7],
                ["dividend_per_share"] = values[8],
                ["dividend_yield"] = values[9],
                ["earnings_per_share"] = values[10],
                ["52_week_high"] = values[11],
                ["52_week_low"] = values[12],
                ["50day_moving_avg"] = values[13],
                ["200day_moving_avg"] = values[14],
                ["price_earnings_ratio"] = values[15],
                ["price_earnings_growth_ratio"] = values[16],
                ["price_sales_ratio"] = values[17],
                ["price_book_ratio"] = values[18],
                ["short_ratio"] = values[19]
            };
            return data;
        }

        public static string GetPrice(string symbol)
        {
            return Request(symbol, "l1");
        }

        public static string GetChange(string symbol)
        {
            return Request(symbol, "c1");
        }

        public static string GetVolume(string symbol)
        {
            return Request(symbol, "v");
        }

        public static string GetAvgDailyVolume(string symbol)
        {
            return Request(symbol, "a2");
        }

        public static string GetStockExchange(string symbol)
        {
            return Request(symbol, "x");
        }

        public static string GetMarketCap(string symbol)
        {
            return Request(symbol, "j1");
        }

        public static string GetBookValue(string symbol)
        {
            return Request(symbol, "b4");
        }

        public static string GetEbitda(string symbol)
        {
            return Request(symbol, "j4");
        }

        public static string GetDividendPerShare(string symbol)
        {
            return Request(symbol, "d");
        }

        public static string GetDividendYield(string symbol)
        {
            return Request(symbol, "y");
        }

        public static string GetEarningsPerShare(string symbol)
        {
            return Request(symbol, "e");
        }

        public static string Get52WeekHigh(string symbol)
        {
            return Request(symbol, "k");
        }

        public static string Get52WeekLow(string symbol)
        {
            return Request(symbol, "j");
        }

        public static string Get50DayMovingAvg(string symbol)
        {
            return Request(symbol, "m3");
        }

        public static string Get200DayMovingAvg(string symbol)
        {
            return Request(symbol, "m4");
        }

        public static string GetPriceEarningsRatio(string symbol)
        {
            return Request(symbol, "r");
        }

        public static string GetpriceEarningsGrowthRatio(string symbol)
        {
            return Request(symbol, "r5");
        }

        public static string GetpriceSalesRatio(string symbol)
        {
            return Request(symbol, "p5");
        }

        public static string GetPriceBookRatio(string symbol)
        {
            return Request(symbol, "p6");
        }

        public static string GetShortRatio(string symbol)
        {
            return Request(symbol, "s7");
        }

        public static List<string> GetHistoricalPrices(string symbol, DateTime start, DateTime end)
        {
            var url = $"http://ichart.yahoo.com/table.csv?s={symbol}&" +
                      $"d={end.Month - 1}&" +
                      $"e={end.Day}&" +
                      $"f={end.Year}&" +
                      "g=d&" +
                      $"a={start.Month - 1}&" +
                      $"b={start.Day}&" +
                      $"c={start.Year}&" +
                      "ignore=.csv";
            var days = Reader.ReadString(url).Split(new char[] {'\t', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            return days.ToList();
        }
    }
}