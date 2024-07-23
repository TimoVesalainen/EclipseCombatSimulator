using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.WinUI.ViewModel;
using EclipseCombatCalculator.Library;
using System.Threading.Tasks;
using EclipseCombatCalculator.Library.Dices;
using EclipseCombatCalculator.WinUI.Dialogs;
using EclipseCombatCalculator.Library.Combat;

namespace EclipseCombatCalculator.WinUI
{
    public sealed partial class CombatPage : Page
    {
        public CombatPage()
        {
            this.InitializeComponent();
            AttackerFleet.Ships.Add(CombatShipType.Create(Blueprint.TerranInterceptor));
            DefenderFleet.Ships.Add(CombatShipType.Create(Blueprint.OrionCruiser));
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var attackerIsAI = !AttackerFleet.ManualFleet;
            var attackerAi = AttackerFleet.SelectedAI.Implementation;
            var defenderIsAI = !DefenderFleet.ManualFleet;
            var defenderAi = DefenderFleet.SelectedAI.Implementation;

            async Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> AssignDamage(
            ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult)
            {
                if (attacker.IsAttacker)
                {
                    if (attackerIsAI)
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
                    if (defenderIsAI)
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
                    if (attackerIsAI)
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
                    if (defenderIsAI)
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
            await foreach (var state in CombatLogic.DoCombat(
                AttackerFleet.Ships.Select(viewModel => (viewModel.Blueprint as IShipStats, viewModel.Count)),
                DefenderFleet.Ships.Select(viewModel => (viewModel.Blueprint as IShipStats, viewModel.Count)),
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

            var dialog = new Dialogs.RetreatAskerDialog
            {
                XamlRoot = this.XamlRoot
            };
            dialog.ViewModel.Ship = activeShips;

            await dialog.ShowAsync();
            return (dialog.ViewModel.StartRetreat, dialog.ViewModel.CompleteRetreat);
        }
    }
}
