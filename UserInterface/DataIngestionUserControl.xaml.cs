using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SeriesTransformation;
using SeriesTransformation.Shared;
using static QuotesConversion.QuotesConverter;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for DataIngestionUserControl.xaml
    /// </summary>
    public partial class DataIngestionUserControl : UserControl
    {
        public DataIngestionUserControl()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            IngestData();
        }

        private void IngestData()
        {
            Dispatcher.InvokeAsync(delegate
            {
                LogTextBox.Text = "Ingesting data";
                LoadButton.IsEnabled = false;
                ISeriesConverter converter = new SeriesConverter();
                var dateTimeParser = new QuoteDateTimeAsTextParser("yyyy.MM.dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                var ingestionPath = $@"{RootPathTextBox.Text}\{AssetTextBox.Text}\{ProviderTextBox.Text}\";
                QuotesProcessor processor = new QuotesProcessor(ingestionPath, dateTimeParser);
                DateTime begin = new DateTime(2000, 1, 1);
                DateTime end = new DateTime(2030, 1, 1);
                var path = TextFileToImportPathTextBox.Text;
                Task.Factory.StartNew(() =>
                {
                    converter.ImportQuotes(path, processor, begin, end);
                    Dispatcher.InvokeAsync(delegate
                    {
                        LogTextBox.Text += Environment.NewLine + "Completed";
                        LoadButton.IsEnabled = true;
                    });
                });

            }).Completed += (sender, args) =>
            {
                LogTextBox.Text = "Ingesting data";
            };
        }

        public IngestionDataSelection DataSelection
        {
            get
            {
                return new IngestionDataSelection(RootPathTextBox.Text, AssetTextBox.Text, ProviderTextBox.Text);
            }
        }
    }
}
