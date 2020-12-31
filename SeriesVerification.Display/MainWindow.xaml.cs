/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using SeriesReading;
using SeriesReading.Descriptor.Quotes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SeriesVerification.Display
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var seriesDescriptor = new SeriesDescriptor();
            this.InstrumentsPanel.ItemsSource = seriesDescriptor.ChildDescriptors;
        }

        private void InstrumentsPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                InstrumentDescriptor descriptor = (InstrumentDescriptor) e.AddedItems[0];
                this.ProvidersPanel.ItemsSource = descriptor.ChildDescriptors;
            }
        }

        private void ProvidersPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ProviderDescriptor descriptor = (ProviderDescriptor) e.AddedItems[0];
                this.YearsPanel.ItemsSource = descriptor.ChildDescriptors;
            }
        }

        private void YearsPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                YearDescriptor descriptor = (YearDescriptor) e.AddedItems[0];
                this.MonthsPanel.ItemsSource = descriptor.ChildDescriptors;
            }
        }

        private void MonthsPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                MonthDescriptor descriptor = (MonthDescriptor) e.AddedItems[0];
                this.DaysPanel.ItemsSource = descriptor.ChildDescriptors;
            }
        }

        private void DaysPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                DayDescriptor descriptor = (DayDescriptor) e.AddedItems[0];
                this.ValuesGrid.Items.Clear();
                SeriesReader reader = SeriesReader.CreateReaderForSingleFile(descriptor.Path);
                DateTime dateTime;
                decimal ask, bid;
                while (reader.Next(out dateTime, out ask, out bid))
                {
                    this.ValuesGrid.Items.Add(new {DateTime = dateTime, Ask = ask, Bid = bid});
                }

                this.StatusText.Text = string.Format("{0} samples...", this.ValuesGrid.Items.Count);
            }
        }
    }
}