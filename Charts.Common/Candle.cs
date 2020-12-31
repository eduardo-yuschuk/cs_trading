/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Charts.Common
{
    public class Candle : IDrawable
    {
        public Candle(DateTime begin, DateTime end, double open, double high, double low, double close)
        {
            Begin = begin;
            End = end;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double MinValue => Low;
        public double MaxValue => High;
        public IStructuredInformation Information => new StructuredInformation(this);

        class StructuredInformation : IStructuredInformation
        {
            private readonly Candle _candle;

            public StructuredInformation(Candle candle)
            {
                _candle = candle;
            }

            public string Header { get; } = "Trade";

            public List<IInformationRow> InformationRows =>
                new List<IInformationRow>
                {
                    new InformationRow("Begin", _candle.Begin.ToString("dd/MM/yyyy HH:mm:ss (fff)")),
                    new InformationRow("End", _candle.End.ToString("dd/MM/yyyy HH:mm:ss (fff)")),
                    new InformationRow("Open", _candle.Open.ToString(CultureInfo.InvariantCulture)),
                    new InformationRow("High", _candle.High.ToString(CultureInfo.InvariantCulture)),
                    new InformationRow("Low", _candle.Low.ToString(CultureInfo.InvariantCulture)),
                    new InformationRow("Close", _candle.Close.ToString(CultureInfo.InvariantCulture)),
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