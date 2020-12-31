/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using FinancialData.Shared;

namespace FinancialData.Manager
{
    public class FinancialDataBuffer
    {
        private int _bufferSize;
        private int _samplesCount = 0;
        private List<IQuote> _quotes = new List<IQuote>();

        public FinancialDataBuffer(int bufferSize)
        {
            _bufferSize = bufferSize;
        }

        public void AddQuote(IQuote quote)
        {
            _quotes.Add(quote);
            if (++_samplesCount == _bufferSize)
            {
                FinancialDataManager.Instance.AddQuotes(_quotes, false);
                _quotes.Clear();
                _samplesCount = 0;
            }
        }
    }
}