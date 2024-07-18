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
using EclipseCombatCalculator.Library.Dices;
using EclipseCombatCalculator.WinUI.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library.Combat;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EclipseCombatCalculator.WinUI.Dialogs
{
    public sealed partial class DiceAssingmentDialog : ContentDialog
    {
        public DiceAssignmentViewModel ViewModel { get; } = new();

        public IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)> Result =>
            ViewModel.Ships.Select(shipVM => (shipVM.Ship, shipVM.AssignedDiceFaces.Select(diceVm => diceVm.Dice)));

        public DiceAssingmentDialog()
        {
            this.InitializeComponent();
        }

        private void Grid_DragStarting(object sender, DragStartingEventArgs e)
        {
            e.Data.SetData("Dice", ((sender as Grid).DataContext as DiceViewModel).ID);
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains("Dice"))
            {
                var diceGuid = (Guid)(await e.DataView.GetDataAsync("Dice"));

                var diceViewModel = this.ViewModel.AllDice.FirstOrDefault(vm => vm.ID == diceGuid);

                var target = (sender as Grid).DataContext as TargetShipViewModel;

                ViewModel.UnAssignedFaces.Remove(diceViewModel);
                foreach (var ship in ViewModel.Ships)
                {
                    ship.AssignedDiceFaces.Remove(diceViewModel);
                }

                target.AssignedDiceFaces.Add(diceViewModel);
            }
        }
    }
}
