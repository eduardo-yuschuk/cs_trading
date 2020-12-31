/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialIndicator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialIndicator
{
    public class IndicatorsCreator
    {
        private List<IndicatorsCreatorItem> _indicatorsCreatorItems = new List<IndicatorsCreatorItem>();
        public TimeSpan TimeFrame { get; private set; }
        public string StoragePath { get; private set; }

        public IndicatorsCreator(TimeSpan timeFrame, string storagePath)
        {
            TimeFrame = timeFrame;
            StoragePath = storagePath;
        }

        public void AddIndicator(IFinancialIndicator indicator)
        {
            var found = _indicatorsCreatorItems.SingleOrDefault(ind =>
                ind.Indicator.Identifier == indicator.Identifier);
            if (found != null)
                _indicatorsCreatorItems.Remove(found);
            _indicatorsCreatorItems.Add(new IndicatorsCreatorItem(TimeFrame, StoragePath, indicator));
        }

        public void Update(DateTime dateTime, decimal price)
        {
            _indicatorsCreatorItems.ToList().ForEach(item => item.Update(dateTime, price));
        }

        public List<decimal> Values
        {
            get { return _indicatorsCreatorItems.Select(item => item.Indicator.Value).ToList(); }
        }

        public void Finish()
        {
            _indicatorsCreatorItems.ToList().ForEach(item => item.Finish());
            VerifyConsistency();
        }

        private void VerifyConsistency()
        {
        }
    }
}