using EclipseCombatCalculator.WinUI.Dialogs;
using EclipseCombatCalculator.WinUI.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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
        public bool CanChooseManual
        {
            get => (bool)GetValue(CanChooseManualProperty);
            set => SetValue(CanChooseManualProperty, value);
        }
        public bool IsAiEnabled => !CanChooseManual || AISwitch.IsOn;
        public Visibility ShowSwitch => CanChooseManual ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ShowText => CanChooseManual ? Visibility.Collapsed : Visibility.Visible;
        public bool HasShips => Ships.Count > 0;
        public bool ManualFleet => !AISwitch.IsOn;
        public AIViewModel SelectedAI => AISelection.SelectedItem as AIViewModel;

        public FleetControl()
        {
            this.InitializeComponent();
            AISelection.IsEnabled = false;
            this.RegisterPropertyChangedCallback(CanChooseManualProperty, (sender, dp) =>
            {
                var value = CanChooseManual;
                if (!value)
                {
                    AISelection.IsEnabled = true;
                }
            });
        }

        private void AISwitch_Toggled(object sender, RoutedEventArgs e)
        {
            AISelection.IsEnabled = AISwitch.IsOn;
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (e.OriginalSource as Button).DataContext as CombatShipType;
            viewModel.Count += 1;
            OnFleetChanged();
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            var combatShipModel = (e.OriginalSource as Button).DataContext as CombatShipType;
            combatShipModel.Count -= 1;
            if (combatShipModel.Count == 0)
            {
                Ships.Remove(combatShipModel);
            }
            else
            {
                OnFleetChanged();
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
                if (Ships.Any(ship => ship.Blueprint == dialog.SelectedItem))
                {
                    var ship = Ships.First(ship => ship.Blueprint == dialog.SelectedItem);
                    ship.Count += 1;
                }
                else
                {
                    Ships.Add(CombatShipType.Create(dialog.SelectedItem));
                }
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

        private readonly DependencyProperty CanChooseManualProperty = DependencyProperty.Register(
             nameof(CanChooseManualProperty),
             typeof(bool),
             typeof(FleetControl),
             new PropertyMetadata(true));

        private void OnFleetChanged()
        {
            FleetChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler FleetChanged;
    }
}
