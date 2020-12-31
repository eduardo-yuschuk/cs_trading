using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BarsReading;
using Charts;
using Charts.Common;
using SeriesReading.Descriptor.Quotes;
using Simulation;
using Simulation.Shared;
using Simulation.Strategies;
using static Simulation.BruteForceSimulationRunner;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for ParametersEvolutionUserControl.xaml
    /// </summary>
    public partial class ParametersEvolutionUserControl : UserControl
    {
        #region Constructors

        public Hook Hook { get; set; }

        //private BruteForceSimulationRunner _bruteForceSimulationRunner;

        public ParametersEvolutionUserControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Data

        private void BuildPeriod(out DateTime begin, out DateTime end)
        {
            var dataSelection = DataSelector.DataSelection;
            var year = dataSelection.Year;
            begin = new DateTime(year, 1, 1, 0, 0, 0);
            end = new DateTime(year, 12, 31, 0, 0, 0) + TimeSpan.FromDays(1);
        }

        private BarsReader CreateReader()
        {
            var dataSelection = DataSelector.DataSelection;
            var path = new SeriesDescriptor()
                .InstrumentDescriptors.Single(x => x.Name == dataSelection.Asset)
                .ProviderDescriptors.Single(x => x.Name == dataSelection.Provider)
                .Path;
            var reader = BarsReader.Create(TimeSpan.FromMinutes(1), path);
            return reader;
        }

        #endregion

        private void StartSearchButton_Click(object sender, RoutedEventArgs e)
        {
            ClearLog();
            Log($"Starting strategy {StrategyComboBox.SelectedItem}...\n");
            ////////////////////////////////////////////////////////////////////////////////
            var reader = CreateReader();
            BuildPeriod(out var begin, out var end);
            var asm = Assembly.Load(SimulationNamespace);
            var simulationType = asm.ExportedTypes.Single(x => x.Name == StrategyComboBox.SelectedItem.ToString());
            var argumentValues = ((List<BruteForceConfigurableArgument>)ArgumentsDataGrid.ItemsSource);
            var bestConfigurations = new List<RankedTestedConfigurations>();
            while (begin < end)
            {
                Console.WriteLine($"{begin}");
                var bars = reader.ReadBetween(begin, begin + TimeSpan.FromDays(1));
                var bruteForceSimulationRunner = new BruteForceSimulationRunner(bars, simulationType, argumentValues);
                bruteForceSimulationRunner.Execute();
                var rankedTestedConfigurations = new RankedTestedConfigurations(begin, begin + TimeSpan.FromDays(1), bruteForceSimulationRunner);
                bestConfigurations.Add(rankedTestedConfigurations);
                begin += TimeSpan.FromDays(1);
            }
            // presento los resultados
            DailyBestConfigurationsDataGrid.ItemsSource = bestConfigurations;
            Log($"Strategy {StrategyComboBox.SelectedItem} executed...\n");
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var series = new List<Series>();
            var colorsProvider = DrawingColorsProvider.New;
            // SMALL MA PERIOD
            var smallIndicatorSeries = new Series("Small", ChartType.Lines, colorsProvider.Next,
                bestConfigurations
                .Where(x => x.Result != 0)
                .Select(
                    x => new Sample(
                        x.Begin, double.Parse(x.TestedConfiguration.ArgumentValues[2].ToString())
                    )
                )
            );
            series.Add(smallIndicatorSeries);
            // BIG MA PERIOD
            var bigIndicatorSeries = new Series("Big", ChartType.Lines, colorsProvider.Next,
                bestConfigurations
                .Where(x => x.Result != 0)
                .Select(
                    x => new Sample(
                        x.Begin, double.Parse(x.TestedConfiguration.ArgumentValues[3].ToString())
                    )
                )
            );
            series.Add(bigIndicatorSeries);
            // PNL
            var pnlSeries = new Series("PnL", ChartType.Lines, colorsProvider.Next,
                bestConfigurations
                .Where(x => x.Result != 0)
                .Select(
                    x => new Sample(
                        x.Begin, double.Parse((x.Result * 1000M).ToString())
                    )
                )
            );
            series.Add(pnlSeries);
            AddSeries(Chart, series, begin, end);
            SaveSeries(series, @"C:\Users\user\");
        }

        private void DisplayResults(DateTime begin, DateTime end, ISimulation simulation, BruteForceSimulationRunner simulationRunner)
        {
            var series = new List<Series>();
            var colorsProvider = DrawingColorsProvider.New;
            // BARS
            var timeFrame = TimeSpan.FromMinutes(1);
            var samples = new List<Candle>();
            foreach (var bar in simulation.Bars)
            {
                samples.Add(new Candle(bar.DateTime, bar.DateTime + timeFrame, (double)bar.Open, (double)bar.High, (double)bar.Low, (double)bar.Close));
            }

            var barSeries = new Series("Bars", ChartType.Bars, Colors.Transparent, samples);
            //DataHelper.CreateBarsSeries(out var begin, out var end, out var barSeries, DataSelector.DataSelection);
            series.Add(barSeries);
            // INDICADORES
            foreach (var serie in simulationRunner.Series)
            {
                var name = serie.Key;
                var values = serie.Value;
                var indicatorSeries = new Series(name, ChartType.Lines, colorsProvider.Next, values.InstantValues.Select(x => new Sample(x.DateTime, x.Value)));
                series.Add(indicatorSeries);
            }

            // TRADES
            var trades = simulation.ClosedPositions.Select(x => new Trade(x.OpenDateTime, x.CloseDateTime, x.Side, (double)x.OpenPrice, (double)x.ClosePrice));
            var tradeSeries = new Series("Trades", ChartType.Trades, Colors.Purple, trades);
            series.Add(tradeSeries);

            AddSeries(Chart, series, begin, end);
        }

        private void AddSeries(Chart chart, List<Series> seriesList, DateTime begin, DateTime end)
        {
            try
            {
                if (seriesList == null) return;
                if (!seriesList.Exists(x => x.Drawables.Count > 0)) return;
                chart.ClearSeries();
                chart.Begin = begin;
                chart.End = end;
                chart.Maximum = seriesList.Where(x => x.Drawables.Count > 0).Max(serie => serie.Drawables.Max(dwbl => dwbl.MaxValue));
                chart.Minimum = seriesList.Where(x => x.Drawables.Count > 0).Min(serie => serie.Drawables.Min(dwbl => dwbl.MinValue));
                seriesList.ForEach(chart.AddSeries);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void SaveSeries(List<Series> seriesList, string path)
        {
            try
            {
                if (seriesList == null) return;
                if (!seriesList.Exists(x => x.Drawables.Count > 0)) return;
                foreach (var series in seriesList)
                {
                    var seriesPath = path + series.Name + ".csv";
                    if (File.Exists(seriesPath)) File.Delete(seriesPath);
                    using (var file = new StreamWriter(File.OpenWrite(seriesPath)))
                    {
                        file.Write($"Time,Value" + Environment.NewLine);
                        foreach (var drawable in series.Drawables)
                        {
                            var sample = (Sample)drawable;
                            file.Write($"{sample.Instant},{sample.Value}" + Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #region Log

        void ClearLog()
        {
            LogTextBox.Text = "";
        }

        void Log(string text)
        {
            LogTextBox.Text += text;
        }

        #endregion

        #region DynamicStrategies

        private const string SimulationNamespace = "Simulation.Strategies";

        private void BruteForceSearchUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            StrategyComboBox.Items.Clear();
            var asm = Assembly.Load(SimulationNamespace);
            foreach (var exportedType in asm.ExportedTypes)
            {
                if (exportedType.Name != "Hook")
                {
                    StrategyComboBox.Items.Add(exportedType.Name);
                }
            }
        }

        private void StrategyComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var asm = Assembly.Load(SimulationNamespace);
            if (StrategyComboBox.SelectedItem == null) return;
            var strategy = asm.ExportedTypes.Single(x => x.Name == StrategyComboBox.SelectedItem.ToString());
            var defaultArgumentsMethod = strategy.GetMethod("GetArgumentsDefault");
            var bruteForceDefaultArguments = BruteForceConfigurableArgument.ToBruteForce((List<ConfigurableArgument>)defaultArgumentsMethod?.Invoke(null, new object[] { }));
            ArgumentsDataGrid.ItemsSource = bruteForceDefaultArguments;
        }

        #endregion

        private void TestedConfigurationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var testedConfiguration = (RankedTestedConfigurations)e.AddedItems[0];
                ISimulation simulation = testedConfiguration.TestedConfiguration.Simulation;
                DisplayResults(testedConfiguration.Begin, testedConfiguration.End, simulation, testedConfiguration.BruteForceSimulationRunner);
                Log($"Report: {simulation.GetReport()}\n");
            }
            catch (Exception) { }
        }

        private class RankedTestedConfigurations
        {
            public DateTime Begin { get; private set; }
            public DateTime End { get; private set; }
            public TestedConfiguration TestedConfiguration { get; private set; }
            public BruteForceSimulationRunner BruteForceSimulationRunner { get; private set; }
            public string Description => $"{Begin} -> {TestedConfiguration.Simulation.Description}";
            public decimal Result => TestedConfiguration.Simulation.Earnings;
            public RankedTestedConfigurations(DateTime begin, DateTime end, BruteForceSimulationRunner bruteForceSimulationRunner)
            {
                Begin = begin;
                End = end;
                //BruteForceSimulationRunner = bruteForceSimulationRunner;
                TestedConfiguration = bruteForceSimulationRunner.TestedConfigurations.OrderByDescending(x => x.Result).First();
            }
        }
    }
}
