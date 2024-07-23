using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library.Combat;
using EclipseCombatCalculator.WinUI.Dialogs;
using EclipseCombatCalculator.WinUI.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EclipseCombatCalculator.WinUI.Controls
{
    public sealed partial class FleetControl : UserControl
    {
        public ObservableCollection<CombatShipType> Ships
        {
            get => (ObservableCollection<CombatShipType>)GetValue(ShipsProperty);
            set => SetValue(ShipsProperty, value);
        }
        public ObservableCollection<AIViewModel> AIs
        {
            get => (ObservableCollection<AIViewModel>)GetValue(AIsProperty);
            set => SetValue(AIsProperty, value);
        }

        public bool CanChooseManual { get; set; } = false;
        public Visibility ShowSwitch => CanChooseManual ? Visibility.Visible : Visibility.Collapsed;
        public bool HasShips => Ships.Count > 0;
        public bool ManualFleet => !AISwitch.IsOn;
        public AIViewModel SelectedAI => AISelection.SelectedItem as AIViewModel;

        public FleetControl()
        {
            this.InitializeComponent();
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (e.OriginalSource as Button).DataContext as CombatShipType;
            viewModel.Count += 1;
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            var combatShipModel = (e.OriginalSource as Button).DataContext as CombatShipType;
            combatShipModel.Count -= 1;
            if (combatShipModel.Count == 0)
            {
                Ships.Remove(combatShipModel);
            }
        }

        private async void AddShip_Click(object sender, RoutedEventArgs e)
        {
            BlueprintSelectionDialog dialog = new()
            {
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Ships.Add(CombatShipType.Create(dialog.SelectedItem));
            }
        }

        private readonly DependencyProperty ShipsProperty = DependencyProperty.Register(
             nameof(Ships),
             typeof(ObservableCollection<CombatShipType>),
             typeof(FleetControl),
              new PropertyMetadata(new ObservableCollection<CombatShipType>()));

        private readonly DependencyProperty AIsProperty = DependencyProperty.Register(
             nameof(AIs),
             typeof(ObservableCollection<AIViewModel>),
             typeof(FleetControl),
              new PropertyMetadata(new ObservableCollection<AIViewModel>()));
    }
}
