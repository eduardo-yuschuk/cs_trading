/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;

namespace YahooStockQuote.FinancialDataProvider
{
    public class YQSDataSource : IDataSource
    {
        public DataProvider Provider => DataProvider.YahooStockQuote;

        public decimal ConvertPrice(decimal price)
        {
            return price;
        }
    }
}