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
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.WinUI.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EclipseCombatCalculator.WinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlueprintsPage : Page
    {
        public BlueprintsViewModel ViewModel { get; } = new();

        public BlueprintsPage()
        {
            this.InitializeComponent();
            this.LayoutList.ItemsSource = Blueprint.Blueprints.Select(LayoutListViewModel.Create);
        }

        private void LayoutList_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedBlueprintView.Visibility = Visibility.Visible;
            ViewModel.SelectedBlueprint = (e.ClickedItem as LayoutListViewModel).Blueprint;
        }
    }
}
