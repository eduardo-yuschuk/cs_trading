/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Charts.Common
{
    public class Sample : IDrawable
    {
        public DateTime Instant { get; set; }
        public double Value { get; set; }

        public Sample()
        {
        }

        public Sample(DateTime instant, double value)
        {
            Instant = instant;
            Value = value;
        }

        public override string ToString()
        {
            return Instant + " " + Value;
        }

        public double MinValue => Value;
        public double MaxValue => Value;
        public DateTime Begin => Instant;
        public DateTime End => Instant;
        public IStructuredInformation Information => new StructuredInformation(this);

        class StructuredInformation : IStructuredInformation
        {
            private readonly Sample _sample;

            public StructuredInformation(Sample trade)
            {
                _sample = trade;
            }

            public string Header { get; } = "Sample";

            public List<IInformationRow> InformationRows =>
                new List<IInformationRow>
                {
                    new InformationRow("Begin", _sample.Begin.ToString("dd/MM/yyyy HH:mm:ss (fff)")),
                    new InformationRow("End", _sample.End.ToString("dd/MM/yyyy HH:mm:ss (fff)")),
                };

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine($"{Header}");
                foreach (var informationRow in InformationRows)
                {
                    sb.AppendLine($"{informationRow.Attribute}: {informationRow.Value}");
                }

                return sb.ToString();
            }
        }

        class InformationRow : IInformationRow
        {
            public string Attribute { get; }
            public string Value { get; }

            public InformationRow(string attribute, string value)
            {
                Attribute = attribute;
                Value = value;
            }
        }
    }
}