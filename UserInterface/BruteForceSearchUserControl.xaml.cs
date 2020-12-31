/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using BarsReading;
using Charts;
using Charts.Common;
using SeriesReading.Descriptor.Quotes;
using Simulation;
using Simulation.Shared;
using Simulation.Strategies;
using System;
using System.Collections.Generic;
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
using static Simulation.BruteForceSimulationRunner;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for BruteForceSearchUserControl.xaml
    /// </summary>
    public partial class BruteForceSearchUserControl : UserControl
    {
        #region Constructors

        public Hook Hook { get; set; }

        private BruteForceSimulationRunner _bruteForceSimulationRunner;

        public BruteForceSearchUserControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Data

        private void BuildPeriod(out DateTime begin, out DateTime end)
        {
            var dataSelection = DataSelector.DataSelection;
            var year = dataSelection.Year;
            var month = dataSelection.Month;
            var days = dataSelection.Days;
            begin = new DateTime(year, month, days.First(), 0, 0, 0);
            end = new DateTime(year, month, days.Last(), 0, 0, 0) + TimeSpan.FromDays(1);
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
            var bars = reader.ReadBetween(begin, end);
            var asm = Assembly.Load(SimulationNamespace);
            var simulationType = asm.ExportedTypes.Single(x => x.Name == StrategyComboBox.SelectedItem.ToString());
            var argumentValues = ((List<BruteForceConfigurableArgument>)ArgumentsDataGrid.ItemsSource);//.Select(x => x.Value).ToArray();
            _bruteForceSimulationRunner = new BruteForceSimulationRunner(bars, simulationType, argumentValues);
            _bruteForceSimulationRunner.Execute();
            // presento los resultados
            TestedConfigurationsDataGrid.ItemsSource = _bruteForceSimulationRunner.TestedConfigurations.OrderByDescending(x => x.Result);
            Log($"Strategy {StrategyComboBox.SelectedItem} executed...\n");
        }

        private void DisplayResults(ISimulation simulation, BruteForceSimulationRunner simulationRunner)
        {
            var series = new List<Series>();
            var colorsProvider = DrawingColorsProvider.New;
            // BARS
            var timeFrame = TimeSpan.FromMinutes(1);
            var samples = new List<Candle>();
            foreach (var bar in simulation.Bars)
            {
                samples.Add(new Candle(bar.DateTime, bar.DateTime + timeFrame, (double)bar.Open, (double)bar.High,
                    (double)bar.Low, (double)bar.Close));
            }

            var barSeries = new Series("Bars", ChartType.Bars, Colors.Transparent, samples);
            //DataHelper.CreateBarsSeries(out var begin, out var end, out var barSeries, DataSelector.DataSelection);
            series.Add(barSeries);
            // INDICADORES
            foreach (var serie in simulationRunner.Series)
            {
                var name = serie.Key;
                var values = serie.Value;
                var indicatorSeries = new Series(name, ChartType.Lines, colorsProvider.Next,
                    values.InstantValues.Select(x => new Sample(x.DateTime, x.Value)));
                series.Add(indicatorSeries);
            }

            // TRADES
            var trades = simulation.ClosedPositions.Select(x =>
                new Trade(x.OpenDateTime, x.CloseDateTime, x.Side, (double)x.OpenPrice, (double)x.ClosePrice));
            var tradeSeries = new Series("Trades", ChartType.Trades, Colors.Purple, trades);
            series.Add(tradeSeries);
            DataHelper.CreatePeriod(out var begin, out var end, DataSelector.DataSelection);
            AddSeries(Chart, series, begin, end);
            ///////////////////////////////////////////////////////////////////////////////////////
            // balance
            var balanceSeries = new List<Series>
            {
                new Series("Balance", ChartType.Lines, Colors.Black,
                    simulation.Balance.Select(x => new Sample(x.DateTime, (double) x.Value))),
            };
            AddSeries(Balance, balanceSeries, begin, end);
        }

        private void AddSeries(Chart chart, List<Series> seriesList, DateTime begin, DateTime end)
        {
            try
            {
                if (seriesList == null) return;
                if (!seriesList.Exists(x => x.Drawables.Count > 0)) return;
                chart.ClearSeries();
                //this.Chart.Begin = seriesList[0].Drawables.First().Begin;
                chart.Begin = begin;
                //this.Chart.End = seriesList[0].Drawables.Last().End;
                chart.End = end;
                //chart.Maximum = seriesList[0].Drawables.Max(x => x.MaxValue);
                chart.Maximum = seriesList.Where(x => x.Drawables.Count > 0)
                    .Max(serie => serie.Drawables.Max(dwbl => dwbl.MaxValue));
                //chart.Minimum = seriesList[0].Drawables.Min(x => x.MinValue);
                chart.Minimum = seriesList.Where(x => x.Drawables.Count > 0)
                    .Min(serie => serie.Drawables.Min(dwbl => dwbl.MinValue));
                seriesList.ForEach(chart.AddSeries);
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
                TestedConfiguration testedConfiguration = (TestedConfiguration)e.AddedItems[0];
                ISimulation simulation = testedConfiguration.Simulation;
                DisplayResults(simulation, _bruteForceSimulationRunner);
                Log($"Report: {simulation.GetReport()}\n");
            }
            catch (Exception) { }
        }
    }
}
