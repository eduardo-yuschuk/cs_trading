/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using Simulation.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charts.Common
{
    public class Trade : IDrawable
    {
        public Trade(DateTime begin, DateTime end, PositionSide side, double openPrice, double closePrice)
        {
            Begin = begin;
            End = end;
            Side = side;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
        }

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public PositionSide Side { get; set; }
        public double OpenPrice { get; set; }
        public double ClosePrice { get; set; }

        public double MinValue
        {
            get { return OpenPrice < ClosePrice ? OpenPrice : ClosePrice; }
        }

        public double MaxValue
        {
            get { return OpenPrice > ClosePrice ? OpenPrice : ClosePrice; }
        }

        public IStructuredInformation Information
        {
            get { return new StructuredInformation(this); }
        }

        class StructuredInformation : IStructuredInformation
        {
            private Trade _trade;

            public StructuredInformation(Trade trade)
            {
                _trade = trade;
            }

            public string Header { get; } = "Trade";

            public List<IInformationRow> InformationRows
            {
                get
                {
                    return new List<IInformationRow>
                    {
                        new InformationRow("Begin", _trade.Begin.ToString("dd/MM/yyyy HH:mm:ss (fff)")),
                        new InformationRow("End", _trade.End.ToString("dd/MM/yyyy HH:mm:ss (fff)")),
                    };
                }
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("{0}", Header));
                foreach (var informationRow in InformationRows)
                {
                    sb.AppendLine(string.Format("{0}: {1}", informationRow.Attribute, informationRow.Value));
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