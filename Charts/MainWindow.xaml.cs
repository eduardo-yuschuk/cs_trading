/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Windows;

namespace Charts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public DateTime Begin
        {
            set { this.Dispatcher.Invoke(() => { this.Chart.Begin = value; }); }
        }

        public DateTime End
        {
            set { this.Dispatcher.Invoke(() => { this.Chart.End = value; }); }
        }

        public double Maximum
        {
            set { this.Dispatcher.Invoke(() => { this.Chart.Maximum = value; }); }
        }

        public double Minimum
        {
            set { this.Dispatcher.Invoke(() => { this.Chart.Minimum = value; }); }
        }

        public void AddSeries(Series series)
        {
            this.Dispatcher.Invoke(() => { this.Chart.AddSeries(series); });
        }

        public void ClearSeries()
        {
            this.Dispatcher.Invoke(() => { this.Chart.ClearSeries(); });
        }
    }
}