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
using System.Collections.ObjectModel;
using EclipseCombatCalculator.Library;
using System.Threading.Tasks;
using EclipseCombatCalculator.Library.Dices;
using EclipseCombatCalculator.WinUI.Dialogs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EclipseCombatCalculator.WinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CombatPage : Page
    {
        public CombatPageViewModel ViewModel { get; } = new();

        public CombatPage()
        {
            this.InitializeComponent();
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (e.OriginalSource as Button).DataContext as CombatShipType;
            viewModel.Count += 1;
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            var combatShipModel = (e.OriginalSource as Button).DataContext as CombatShipType;
            combatShipModel.Count -= 1;
            if (combatShipModel.Count == 0)
            {
                ViewModel.Remove(combatShipModel);
            }
        }

        private async void AddAttacker_Click(object sender, RoutedEventArgs e)
        {
            BlueprintSelectionDialog dialog = new()
            {
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.Attackers.Add(CombatShipType.Create(dialog.SelectedItem));
            }
        }

        private async void AddDefender_Click(object sender, RoutedEventArgs e)
        {
            BlueprintSelectionDialog dialog = new()
            {
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ViewModel.Defenders.Add(CombatShipType.Create(dialog.SelectedItem));
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var attackerAi = (AttackerAISelection.SelectedItem as AIViewModel).Implementation;
            var defenderAi = (DefenderAISelection.SelectedItem as AIViewModel).Implementation;

            async Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> AssignDamage(
            ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult)
            {
                if (attacker.IsAttacker)
                {
                    if (AttackerAI.IsOn)
                    {
                        return await attackerAi(attacker, targets, diceResult);
                    }
                    else
                    {
                        return await ManualAssignment(attacker, targets, diceResult);
                    }
                }
                else
                {
                    if (DefenderAI.IsOn)
                    {
                        return await defenderAi(attacker, targets, diceResult);
                    }
                    else
                    {
                        return await ManualAssignment(attacker, targets, diceResult);
                    }
                }
            }

            //TODO: Disable/Hide UI.

            var result = await Combat.AttackerWin(
                ViewModel.Attackers.Select(viewModel => (viewModel.Blueprint as IShipStats, viewModel.Count)),
                ViewModel.Defenders.Select(viewModel => (viewModel.Blueprint as IShipStats, viewModel.Count)),
                AssignDamage, (a) => Task.FromResult((0, 0)));

            ContentDialog resultDialog = new()
            {
                Title = "Combat results",
                Content = result ? "Attacker wins" : "Defender winds",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot,
            };

            await resultDialog.ShowAsync();
            //TODO: Enable/show UI.
        }

        async Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> ManualAssignment(
            ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult)
        {
            // TODO: Ask with UI
            return [];
        }
    }
}
