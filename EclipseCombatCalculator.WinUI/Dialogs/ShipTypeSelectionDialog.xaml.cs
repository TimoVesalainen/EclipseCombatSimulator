using Microsoft.UI.Xaml.Controls;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.WinUI.ViewModel;
using Microsoft.UI.Xaml;
using EclipseCombatCalculator.Library;

namespace EclipseCombatCalculator.WinUI.Dialogs
{
    public sealed partial class ShipTypeSelectionDialog : ContentDialog
    {
        public ShipTypeSelectionViewModel ViewModel { get; } = new();

        public IShipTypeStats SelectedItem => this.ViewModel.SelectedItem;

        public ShipTypeSelectionDialog()
        {
            foreach (var item in Blueprint.Blueprints)
            {
                ViewModel.ShipTypes.Add(ShipTypeViewModel.Create(item));
            }
            foreach (var item in NPCShip.Ships)
            {
                ViewModel.ShipTypes.Add(ShipTypeViewModel.Create(item));
            }
            var app = Application.Current as App;
            foreach (var item in app.CustomBlueprints)
            {
                ViewModel.ShipTypes.Add(ShipTypeViewModel.Create(item));
            }
            this.InitializeComponent();
        }

        private void BlueprintList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (e.ClickedItem as ShipTypeViewModel).Blueprint;
        }
    }
}
