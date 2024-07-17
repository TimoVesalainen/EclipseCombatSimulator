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

            async Task<(int startRetreat, int completeRetreat)> RetreatAsker(ICombatShip activeShips)
            {
                if (activeShips.IsAttacker)
                {
                    if (AttackerAI.IsOn)
                    {
                        return (0, 0);
                    }
                    else
                    {
                        return await Retreater(activeShips);
                    }
                }
                else
                {
                    if (DefenderAI.IsOn)
                    {
                        return (0, 0);
                    }
                    else
                    {
                        return await Retreater(activeShips);
                    }
                }
            }

            //TODO: Disable/Hide UI.

            bool result = false;
            await foreach (var state in Combat.DoCombat(
                ViewModel.Attackers.Select(viewModel => (viewModel.Blueprint as IShipStats, viewModel.Count)),
                ViewModel.Defenders.Select(viewModel => (viewModel.Blueprint as IShipStats, viewModel.Count)),
                AssignDamage, RetreatAsker))
            {
                string PriorityLine(ICombatShip ship)
                {
                    var prefix = state.Active == ship ? "=> " : "";
                    return $"{prefix}{ship.Blueprint.Name} in combat {ship.InCombat} in retreat {ship.InRetreat} retreated {ship.Retreated} destroyed {ship.Defeated}";
                }

                PriorityList.Text = string.Join("\n", state.EngagementRoundOrder.Select(PriorityLine));

                AttackerState.Text = string.Join("\n", state.Attackers.Where(ship => ship.InCombat > 0).Select(ship => $"{ship.Blueprint.Name} count {ship.InCombat} damage {ship.Damage}"));
                DefenderState.Text = string.Join("\n", state.Defenders.Where(ship => ship.InCombat > 0).Select(ship => $"{ship.Blueprint.Name} count {ship.InCombat} damage {ship.Damage}"));

                await Task.Delay(TimeSpan.FromSeconds(1));

                if (state.Ended)
                {
                    result = state.AttackerWinner.Value;
                }
            }

            ContentDialog resultDialog = new()
            {
                Title = "Combat results",
                Content = result ? "Attacker wins" : "Defender winds",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot,
            };

            await resultDialog.ShowAsync();

            PriorityList.Text = "";
            AttackerState.Text = "";
            DefenderState.Text = "";

            //TODO: Enable/show UI.
        }

        async Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> ManualAssignment(
            ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult)
        {
            if (!diceResult.Any())
            {
                return [];
            }

            var dialog = new DiceAssingmentDialog
            {
                XamlRoot = this.XamlRoot
            };

            foreach (var dice in diceResult)
            {
                var viewModel = new DiceViewModel { Dice = dice };
                dialog.ViewModel.AllDice.Add(viewModel);
                dialog.ViewModel.UnAssignedFaces.Add(viewModel);
            }
            foreach (var target in targets)
            {
                dialog.ViewModel.Ships.Add(TargetShipViewModel.Create(target));
            }
            var attackerVM = CombatShipType.Create(attacker.Blueprint as Blueprint);
            attackerVM.Count = attacker.InCombat;
            dialog.ViewModel.AttackerShip = attackerVM;

            await dialog.ShowAsync();

            return dialog.Result;
        }

        async Task<(int startRetreat, int completeRetreat)> Retreater(ICombatShip activeShips)
        {
            if (activeShips.InCombat == 0 && activeShips.InRetreat == 0)
            {
                return (0, 0);
            }

            var dialog = new Dialogs.RetreatAsker
            {
                XamlRoot = this.XamlRoot
            };
            dialog.ViewModel.Ship = activeShips;

            await dialog.ShowAsync();
            return (dialog.ViewModel.StartRetreat, dialog.ViewModel.CompleteRetreat);
        }
    }
}
