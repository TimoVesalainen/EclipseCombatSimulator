using Microsoft.UI.Xaml.Controls;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.WinUI.ViewModel;

namespace EclipseCombatCalculator.WinUI.Dialogs
{
    public sealed partial class BlueprintSelectionDialog : ContentDialog
    {
        public BlueprintsSelectionViewModel ViewModel { get; } = new();

        public Blueprint SelectedItem => this.ViewModel.SelectedItem;

        public BlueprintSelectionDialog()
        {
            foreach (var item in Blueprint.Blueprints)
            {
                ViewModel.Blueprints.Add(LayoutListViewModel.Create(item));
            }
            this.InitializeComponent();
        }

        private void BlueprintList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (e.ClickedItem as LayoutListViewModel).Blueprint;
        }
    }
}
