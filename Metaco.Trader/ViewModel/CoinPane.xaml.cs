﻿using Metaco.Trader.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using Xceed.Wpf.DataGrid;

namespace Metaco.Trader.ViewModel
{
    /// <summary>
    /// Interaction logic for CoinPane.xaml
    /// </summary>
    public partial class CoinPane : UserControl
    {
        public CoinPane()
        {
            InitializeComponent();
            ViewModel = App.Locator.Resolve<CoinsViewModel>();
            grid.SelectionChanged += grid_SelectionChanged;
            coins.CreateCommandBinding(NavigationCommands.Refresh, new Binding("Search"));
        }


        public CoinsViewModel ViewModel
        {
            get
            {
                return coins.DataContext as CoinsViewModel;
            }
            set
            {
                coins.DataContext = value;
            }
        }

        void grid_SelectionChanged(object sender, Xceed.Wpf.DataGrid.DataGridSelectionChangedEventArgs e)
        {
            CoinSelectionChanged(grid);
        }

        public static void CoinSelectionChanged(DataGridControl grid)
        {
            if (grid.SelectedItems.Count == 1)
            {

                var coin = grid.SelectedItems[0] as CoinViewModel;
                if (coin != null)
                {
                    App.Locator.Messenger.Send(new ExposePropertiesMessage(coin.ForPropertyGrid()));
                }
            }
            else
            {
                var coins = grid.SelectedItems.OfType<CoinViewModel>().ToArray();
                App.Locator.Messenger.Send(new ExposePropertiesMessage(new CoinsPropertyViewModel(coins)));
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            
        }

        protected void DataRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var coin = ((CoinViewModel)((DataRow)sender).DataContext);
            coin.Selected();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var popupCoin = ((FrameworkElement)sender).DataContext as CoinViewModel;
            if (popupCoin != null)
                popupCoin.Selected();
            foreach (var coin in grid.SelectedItems.OfType<CoinViewModel>())
            {
                coin.Selected();
            }
        }
    }
}
