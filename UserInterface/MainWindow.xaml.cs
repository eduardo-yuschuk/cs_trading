/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Windows;
using TradingConfiguration.Shared;
using UserInterface.Nodes;

namespace UserInterface
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
            var assetsConfiguration = Configuration.Instance.AssetsConfiguration;
            //assetsConfiguration.AssetsTypes.ForEach(
            ViewModel viewModel = new ViewModel();

            var assets = new CurrenciesNode("Assets");
            assets.AddNode(new CurrenciesNode("Currencies"));
            assets.AddNode(new CurrenciesNode("Stocks"));

            viewModel.Nodes.Add(assets);
            this.DataContext = viewModel;
        }

        private void ElementsTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            INode node = (INode) e.NewValue;
            ShowNodeMenu(node);
        }

        private void ShowNodeMenu(INode node)
        {
            if (node is CurrencyNode)
            {
                ShowCurrencyEditor((CurrencyNode) node);
            }
        }

        private void ShowCurrencyEditor(CurrencyNode node)
        {
        }
    }
}