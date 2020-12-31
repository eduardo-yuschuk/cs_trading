/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;

namespace DukascopyQuote.FinancialDataProvider
{
    public class DukascopyDataSource : IDataSource
    {
        private decimal _priceScale = 1; //100000;

        public DataProvider Provider
        {
            get { return DataProvider.Dukascopy; }
        }

        public decimal ConvertPrice(decimal price)
        {
            return price / _priceScale;
        }
    }
}