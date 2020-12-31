/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Charts;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for DataInspectionUserControl.xaml
    /// </summary>
    public partial class DataInspectionUserControl
    {
        public DataInspectionUserControl()
        {
            InitializeComponent();
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            DataHelper.CreateQuotesSeries(out var begin, out var end, out var quotes, DataSelector.DataSelection);
            AddSeries(Chart, new List<Series> {quotes}, begin, end);
        }

        private void DrawBarsButton_Click(object sender, RoutedEventArgs e)
        {
            DataHelper.CreateBarsSeries(out var begin, out var end, out var bars, DataSelector.DataSelection);
            AddSeries(Chart, new List<Series> {bars}, begin, end);
        }

        private void DrawBothButton_Click(object sender, RoutedEventArgs e)
        {
            DataHelper.CreateQuotesSeries(out var beginQuotes, out var endQuotes, out var quotes,
                DataSelector.DataSelection);
            DataHelper.CreateBarsSeries(out var beginBars, out var endBars, out var bars, DataSelector.DataSelection);
            AddSeries(Chart, new List<Series> {bars, quotes}, beginQuotes < beginBars ? beginQuotes : beginBars,
                endQuotes > endBars ? endQuotes : endBars);
        }

        private void AddSeries(Chart chart, List<Series> seriesList, DateTime begin, DateTime end)
        {
            if (seriesList == null) return;
            chart.ClearSeries();
            //this.Chart.Begin = seriesList[0].Drawables.First().Begin;
            chart.Begin = begin;
            //this.Chart.End = seriesList[0].Drawables.Last().End;
            chart.End = end;
            chart.Maximum = seriesList[0].Drawables.Max(x => x.MaxValue);
            chart.Minimum = seriesList[0].Drawables.Min(x => x.MinValue);
            seriesList.ForEach(chart.AddSeries);
        }
    }
}