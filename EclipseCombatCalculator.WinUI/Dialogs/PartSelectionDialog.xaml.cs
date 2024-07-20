using System.Linq;
using Microsoft.UI.Xaml.Controls;
using EclipseCombatCalculator.WinUI.ViewModel;
using EclipseCombatCalculator.Library.Blueprints;

namespace EclipseCombatCalculator.WinUI.Dialogs
{
    public sealed partial class PartSelectionDialog : ContentDialog
    {
        Part selectedPart;
        public Part SelectedPart
        {
            get => selectedPart;
            set
            {
                selectedPart = value;
                var selectedViewModel = ViewModel.Parts.FirstOrDefault(vm => vm.Part == value);
                ((this.Content as Grid).Children[0] as ListView).SelectedItem = selectedViewModel;
            }
        }

        public PartSelectionViewModel ViewModel { get; } = new();

        public PartSelectionDialog()
        {
            this.InitializeComponent();
        }

        private void PartsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedPart = (e.ClickedItem as PartViewModel).Part;
        }
    }
}
