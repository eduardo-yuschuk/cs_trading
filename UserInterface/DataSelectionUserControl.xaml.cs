/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for DataSelectionUserControl.xaml
    /// </summary>
    public partial class DataSelectionUserControl
    {
        public DataSelectionUserControl()
        {
            InitializeComponent();
        }

        private void DataSelectionUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadAssets();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAssets();
        }

        class FileSystemItem
        {
            public string Path { get; private set; }

            public static FileSystemItem Create(string path)
            {
                return new FileSystemItem { Path = path };
            }

            public override string ToString()
            {
                DirectoryInfo info = new DirectoryInfo(Path);
                return info.Name;
            }
        }

        private void LoadAssets()
        {
            EnableCombos(false);
            Dispatcher.InvokeAsync(delegate
            {
                AssetComboBox.Items.Clear();
                if (!Directory.Exists(RootPathTextBox.Text))
                {
                    Directory.CreateDirectory(RootPathTextBox.Text);
                }
                foreach (var directory in Directory.EnumerateDirectories(RootPathTextBox.Text))
                {
                    AssetComboBox.Items.Add(FileSystemItem.Create(directory));
                }
            }).Completed += (sender, args) =>
            {
                EnableCombos(true);
                AssetComboBox.SelectedIndex = 0;
                ProviderComboBox.SelectedIndex = 0;
                YearComboBox.SelectedIndex = 0;
                MonthComboBox.SelectedIndex = 0;
                DaysListBox.SelectedIndex = 0;
            };
        }

        private void EnableCombos(bool isEnabled)
        {
            MonthComboBox.IsEnabled =
                MonthComboBox.IsEnabled = YearComboBox.IsEnabled = AssetComboBox.IsEnabled = isEnabled;
        }

        private void AssetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDependant(AssetComboBox, ProviderComboBox, Directory.EnumerateDirectories);
        }

        private void ProviderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDependant(ProviderComboBox, YearComboBox, Directory.EnumerateDirectories);
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDependant(YearComboBox, MonthComboBox, Directory.EnumerateDirectories);
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDependant(MonthComboBox, DaysListBox, Directory.EnumerateFiles);
        }

        private static void UpdateDependant(Selector changed, ItemsControl dependant,
            Func<string, IEnumerable<string>> enumeratorMethod)
        {
            dependant.Items.Clear();
            var item = (FileSystemItem)changed.SelectedItem;
            if (item == null) return;
            foreach (var directory in enumeratorMethod(item.Path))
            {
                dependant.Items.Add(FileSystemItem.Create(directory));
            }
        }

        public DataSelection DataSelection
        {
            get
            {
                var days = (from object item in DaysListBox.SelectedItems select item.ToString()).ToList();
                return new DataSelection(RootPathTextBox.Text, AssetComboBox.Text, ProviderComboBox.Text,
                    YearComboBox.Text, MonthComboBox.Text, days);
            }
        }
    }
}