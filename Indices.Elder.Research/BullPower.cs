/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData;
using FinancialData.Shared;
using System;
using System.Collections.Generic;
using TaLib.Extension;

namespace Indices.Elder.Research
{
    public class BullPower
    {
        public List<IInstantValue<double>> InstantValues { get; private set; }
        public int FirstValidSample { get; private set; }

        public int Count
        {
            get { return InstantValues.Count; }
        }

        public BullPower(List<IBar> bars, int period)
        {
            InstantValues = new List<IInstantValue<double>>();
            TaResult ema = bars.EMA(period);
            int count = ema.Count;
            if (count != bars.Count)
            {
                throw new Exception("Samples count can't be different");
            }

            FirstValidSample = ema.FirstValidSample;
            for (int i = 0; i < count; i++)
            {
                DateTime dateTime = bars[i].DateTime;
                double bullPower = 0;
                if (i >= FirstValidSample)
                {
                    double high = (double) bars[i].High;
                    double emaValue = ema.InstantValues[i].Value;
                    bullPower = high - emaValue;
                }

                InstantValues.Add(new InstantValue<double>(dateTime, bullPower));
            }
        }
    }
}