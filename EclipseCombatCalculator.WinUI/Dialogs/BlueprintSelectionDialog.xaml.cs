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
using System.Collections.ObjectModel;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.WinUI.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
