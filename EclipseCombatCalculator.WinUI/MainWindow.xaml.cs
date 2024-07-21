using EclipseCombatCalculator.WinUI.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace EclipseCombatCalculator.WinUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer == Blueprints)
            {
                rootFrame.Navigate(typeof(BlueprintsPage), args, null);
            }
            if (args.SelectedItemContainer == Combat)
            {
                rootFrame.Navigate(typeof(CombatPage), args, null);
            }
            if (args.SelectedItemContainer == Calculations)
            {
                rootFrame.Navigate(typeof(CalculationsPage), args, null);
            }
        }
    }
}
