/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData;
using FinancialData.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaLib.Extension
{
    public class TaResult
    {
        public TaResult() { }
        public TaResult(double[] values, int firstValidSample)
        {
            InstantValues = values.Select(x => new InstantValue<double>(x) as IInstantValue<double>)
                .ToList();
            FirstValidSample = firstValidSample;
        }

        public List<IInstantValue<double>> InstantValues { get; private set; }
        public int FirstValidSample { get; private set; }

        public TaResult Clone()
        {
            var result = new TaResult();
            result.InstantValues = InstantValues.Select(x => x.Clone()).ToList();
            result.FirstValidSample = FirstValidSample;
            return result;
        }

        public void SetDateTimes(List<DateTime> dateTimes)
        {
            int count = InstantValues.Count;
            if (InstantValues.Count != dateTimes.Count)
            {
                throw new Exception("Wrong values qty.");
            }

            for (int i = 0; i < count; i++)
            {
                ((InstantValue<double>)InstantValues[i]).DateTime = dateTimes[i];
            }
        }

        public int Count
        {
            get { return InstantValues.Count; }
        }

        public void Normalize()
        {
            double sum = 0.0;
            for (int i = 0; i < Count; ++i)
            {
                sum += InstantValues[i].Value;
            }

            double mean = sum / Count;
            sum = 0.0;
            for (int i = 0; i < Count; ++i)
            {
                sum += (InstantValues[i].Value - mean) * (InstantValues[i].Value - mean);
            }

            double sd = Math.Sqrt(sum / (Count - 1));
            for (int i = 0; i < Count; ++i)
            {
                InstantValues[i].NormalizedValue = (InstantValues[i].Value - mean) / sd;
            }
        }

        public TaResult ToNormalizedBetweenZeroAndOne()
        {
            var clone = this.Clone();
            double valuesMax = clone.InstantValues.Select(x => x.Value).Max();
            double valuesMin = clone.InstantValues.Select(x => x.Value).Min();
            double delta = valuesMax - valuesMin;

            for (int i = 0; i < Count; ++i)
            {
                var value = (clone.InstantValues[i].Value - valuesMin) / delta;
                clone.InstantValues[i] = new InstantValue<double>(clone.InstantValues[i].DateTime, value) as IInstantValue<double>;
            }

            return clone;
        }

        public void DiscardFirstSamples(int count)
        {
            InstantValues.RemoveRange(0, count);
            FirstValidSample = 0;
        }

        public static int GetFirstValidSample(List<TaResult> resultsList)
        {
            int firstValidSampleOnSeries = 0;
            foreach (TaResult result in resultsList)
            {
                if (result.FirstValidSample > firstValidSampleOnSeries)
                {
                    firstValidSampleOnSeries = result.FirstValidSample;
                }
            }

            return firstValidSampleOnSeries;
        }

        public static void DiscardFirstSamples(List<TaResult> resultsList, int firstValidSampleOnSeries)
        {
            foreach (TaResult result in resultsList)
            {
                result.DiscardFirstSamples(firstValidSampleOnSeries);
            }
        }
    }
}