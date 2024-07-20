using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using EclipseCombatCalculator.WinUI.ViewModel;
using EclipseCombatCalculator.Library.Blueprints;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
