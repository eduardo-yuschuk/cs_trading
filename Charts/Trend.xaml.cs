/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Charts.Common;

namespace Charts
{
    /// <summary>
    /// Interaction logic for Trend.xaml
    /// </summary>
    public partial class Trend : UserControl
    {
        public Trend()
        {
            InitializeComponent();
        }

        Dictionary<Series, FrameworkElement> SeriesToFrameworkElementMapping =
            new Dictionary<Series, FrameworkElement>();
        //public string ChartType { get; set; }

        public void AddSeries(Series series)
        {
            //var begin = DateTime.Now;
            FrameworkElement element = null;
            if (series.ChartType == ChartType.Lines)
            {
                element = Series.GetRenderingLine(series, this.ActualWidth, this.ActualHeight, this.Begin, this.End,
                    this.Minimum, this.Maximum, false);
            }
            else if (series.ChartType == ChartType.DotsAndLines)
            {
                element = Series.GetRenderingLine(series, this.ActualWidth, this.ActualHeight, this.Begin, this.End,
                    this.Minimum, this.Maximum, true);
            }
            else if (series.ChartType == ChartType.Columns)
            {
                element = Series.GetRenderingColumns(series, this.ActualWidth, this.ActualHeight, this.Begin, this.End,
                    this.Minimum, this.Maximum);
            }
            else if (series.ChartType == ChartType.Trades)
            {
                element = Series.GetRenderingTrades(series, this.ActualWidth, this.ActualHeight, this.Begin, this.End,
                    this.Minimum, this.Maximum);
            }
            else if (series.ChartType == ChartType.Bars)
            {
                element = Series.GetRenderingCandles(series, this.ActualWidth, this.ActualHeight, this.Begin, this.End,
                    this.Minimum, this.Maximum);
            }
            else
            {
                throw new Exception($"ChartType {series.ChartType} desconocido...");
            }

            this.SeriesToFrameworkElementMapping[series] = element;
            this.RootPane.Children.Add(element);
            //Console.WriteLine("Delay: {0}", DateTime.Now - begin);
        }

        public void RemoveSeries(Series series)
        {
            this.RootPane.Children.Remove(this.SeriesToFrameworkElementMapping[series]);
            this.SeriesToFrameworkElementMapping.Remove(series);
        }

        public void RemoveAllSeries()
        {
            this.RootPane.Children.Clear();
            this.SeriesToFrameworkElementMapping.Clear();
        }

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public double Maximum { get; set; }
        public double Minimum { get; set; }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            Redraw();
        }

        public void Redraw()
        {
            //var begin = DateTime.Now;
            this.RootPane.Children.Clear();
            this.SeriesToFrameworkElementMapping.Keys.ToList().ForEach(series => { this.AddSeries(series); });
            //Console.WriteLine("OnRenderSizeChanged: {0}", DateTime.Now - begin);
        }
    }
}