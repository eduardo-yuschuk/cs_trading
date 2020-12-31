/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialData.Shared;
using System;

namespace FinancialData
{
    public class InstantValue<T> : IInstantValue<T>
    {
        public DateTime DateTime { get; set; }
        public T Value { get; set; }
        public T NormalizedValue { get; set; }

        public InstantValue(T value)
        {
            Value = value;
        }

        public InstantValue(DateTime dateTime, T value)
            : this(value)
        {
            DateTime = dateTime;
        }

        public SampleType SampleType => SampleType.Raw;

        public IInstantValue<T> Clone()
        {
            var clone = new InstantValue<T>(DateTime, Value);
            clone.NormalizedValue = NormalizedValue;
            return clone;
        }
    }
}