using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
        public readonly CombatPageViewModel ViewModel = new();

        public CombatPage()
        {
            this.InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var attackerIsAI = !AttackerFleet.ManualFleet;
            var attackerAi = AttackerFleet.SelectedAI.Implementation;
            var defenderIsAI = !DefenderFleet.ManualFleet;
            var defenderAi = DefenderFleet.SelectedAI.Implementation;

            async Task<IEnumerable<(ICombatShip, IEnumerable<DiceFace>)>> AssignDamage(
                IShipTypeStats activeShipBlueprint, bool isAttacker,
                IEnumerable<ICombatShip> targets, IEnumerable<DiceFace> diceResult)
            {
                if (isAttacker)
                {
                    if (attackerIsAI)
                    {
                        return await attackerAi(activeShipBlueprint, isAttacker, targets, diceResult);
                    }
                    else
                    {
                        return await ManualAssignment(activeShipBlueprint, isAttacker, targets, diceResult);
                    }
                }
                else
                {
                    if (defenderIsAI)
                    {
                        return await defenderAi(activeShipBlueprint, isAttacker, targets, diceResult);
                    }
                    else
                    {
                        return await ManualAssignment(activeShipBlueprint, isAttacker, targets, diceResult);
                    }
                }
            }

            async Task<IEnumerable<(ICombatShip ship, ShipCombatState newState)>> RetreatAsker(bool attacker, IEnumerable<ICombatShip> ships)
            {
                if (attacker)
                {
                    if (attackerIsAI)
                    {
                        return [];
                    }
                    else
                    {
                        return await Retreater(attacker, ships);
                    }
                }
                else
                {
                    if (defenderIsAI)
                    {
                        return [];
                    }
                    else
                    {
                        return await Retreater(attacker, ships);
                    }
                }
            }

            //TODO: Disable/Hide UI.

            bool result = false;
            await foreach (var state in CombatLogic.DoCombat(
                ViewModel.Attackers.Select(viewModel => (viewModel.ShipType, viewModel.Count)),
                ViewModel.Defenders.Select(viewModel => (viewModel.ShipType, viewModel.Count)),
                AssignDamage, RetreatAsker))
            {
                string PriorityLine(IShipTypeStats stats, bool isAttacker, IEnumerable<ICombatShip> ships)
                {
                    return "";
                    // var prefix = state.Active == ship ? "=> " : "";
                    // return $"{prefix}{ship.Blueprint.Name} in combat {ship.InCombat} in retreat {ship.InRetreat} retreated {ship.Retreated} destroyed {ship.Defeated}";
                }

                PriorityList.Text = string.Join("\n", state.EngagementRoundOrder.Select(tuple => PriorityLine(tuple.Item1, tuple.Item2, tuple.Item3)));

                //AttackerState.Text = string.Join("\n", state.Attackers.Where(ship => ship.InCombat > 0).Select(ship => $"{ship.Blueprint.Name} count {ship.InCombat} damage {ship.Damage}"));
                //DefenderState.Text = string.Join("\n", state.Defenders.Where(ship => ship.InCombat > 0).Select(ship => $"{ship.Blueprint.Name} count {ship.InCombat} damage {ship.Damage}"));

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

        async Task<IEnumerable<(ICombatShip, IEnumerable<DiceFace>)>> ManualAssignment(
                IShipTypeStats activeShipBlueprint, bool isAttacker,
                IEnumerable<ICombatShip> targets, IEnumerable<DiceFace> diceResult)
        {
            if (!diceResult.Any())
            {
                return [];
            }

            var dialog = new DiceAssignmentDialog
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
            var attackerVM = CombatShipType.Create(activeShipBlueprint);
            //attackerVM.Count = attacker.InCombat;
            dialog.ViewModel.AttackerShip = attackerVM;

            await dialog.ShowAsync();

            return dialog.Result;
        }

        async Task<IEnumerable<(ICombatShip ship, ShipCombatState newState)>> Retreater(bool attacker, IEnumerable<ICombatShip> ships)
        {
            if (!ships.Any(ship => ship.State == ShipCombatState.Combat || ship.State == ShipCombatState.Retreating))
            {
                return [];
            }

            var dialog = new RetreatAskerDialog
            {
                XamlRoot = this.XamlRoot
            };
            // dialog.ViewModel.Ship = ships.First().Blueprint;

            await dialog.ShowAsync();
            return [];
            // return (dialog.ViewModel.StartRetreat, dialog.ViewModel.CompleteRetreat);
        }
    }
}
