using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.WinUI.ViewModel;
using Nintenlord.Collections.Lists;

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
            var app = Application.Current as App;
            foreach (var blueprint in app.CustomBlueprints)
            {
                ViewModel.Blueprints.Add(LayoutListViewModel.Create(blueprint));
            }
        }

        private void LayoutList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Blueprint blueprint = (e.ClickedItem as LayoutListViewModel).Blueprint;
            ViewModel.SelectedBlueprint = blueprint;
        }

        private void CloneBlueprint_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            var newBlueprint = ViewModel.SelectedBlueprint.CreateEditableClone();
            newBlueprint.Name = ViewModel.SelectedBlueprint.Name + " (Clone)";

            app.CustomBlueprints.Add(newBlueprint);
            ViewModel.SelectedBlueprint = newBlueprint;
            var viewModel = LayoutListViewModel.Create(newBlueprint);
            ViewModel.Blueprints.Add(viewModel);
            LayoutList.SelectedItem = viewModel;
            LayoutList.ScrollIntoView(viewModel);
        }

        private void BlueprintName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ViewModel.SelectedBlueprint.CanEdit)
            {
                // Happens when changing from editable blueprint to read-only
                return;
            }
            ViewModel.SelectedBlueprint.Name = (sender as TextBox).Text;
        }

        private void SelectedBlueprintView_BlueprintEdited(object sender, EventArgs e)
        {
            ViewModel.UpdateWarnings();
        }

        private void DeleteBlueprint_Click(object sender, RoutedEventArgs e)
        {
            var blueprintToDelete = ViewModel.SelectedBlueprint;

            var app = Application.Current as App;
            app.CustomBlueprints.Remove(blueprintToDelete);
            var index = ViewModel.Blueprints.IndexOf(viewModel => viewModel.Blueprint == blueprintToDelete);
            ViewModel.Blueprints.RemoveAt(index);
            ViewModel.SelectedBlueprint = null;
        }
    }
}
