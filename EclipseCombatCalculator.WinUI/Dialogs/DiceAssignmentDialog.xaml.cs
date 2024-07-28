using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using EclipseCombatCalculator.Library.Dices;
using EclipseCombatCalculator.WinUI.ViewModel;
using Windows.ApplicationModel.DataTransfer;
using EclipseCombatCalculator.Library.Combat;
using EclipseCombatCalculator.Library;

namespace EclipseCombatCalculator.WinUI.Dialogs
{
    public sealed partial class DiceAssignmentDialog : ContentDialog
    {
        public DiceAssignmentViewModel ViewModel { get; } = new();

        public IEnumerable<(ICombatShip, IEnumerable<DiceFace>)> Result =>
            ViewModel.Ships.Select(shipVM => (shipVM.Ship, shipVM.AssignedDiceFaces.Select(diceVm => diceVm.Dice)));

        public DiceAssignmentDialog()
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

                diceViewModel.CanHit = ViewModel.AttackerShip.Blueprint.CanHit(target.Blueprint, diceViewModel.Dice);
                target.AssignedDiceFaces.Add(diceViewModel);
            }
        }
    }
}
