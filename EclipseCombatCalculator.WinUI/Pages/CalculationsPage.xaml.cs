using EclipseCombatCalculator.Library;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library.Combat;
using EclipseCombatCalculator.Library.Dices;
using EclipseCombatCalculator.WinUI.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Nintenlord.Collections.Foldable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EclipseCombatCalculator.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalculationsPage : Page
    {
        public CalculationsViewModel ViewModel { get; } = new();

        public CalculationsPage()
        {
            this.InitializeComponent();
            AttackerFleet.Ships.Add(CombatShipType.Create(Blueprint.TerranInterceptor));
            DefenderFleet.Ships.Add(CombatShipType.Create(Blueprint.OrionCruiser));
            ViewModel.Attackers.CollectionChanged += Fleet_CollectionChanged;
            ViewModel.Defenders.CollectionChanged += Fleet_CollectionChanged;
            AttackerFleet.FleetChanged += Fleet_CollectionChanged2;
            DefenderFleet.FleetChanged += Fleet_CollectionChanged2;
        }

        private void Fleet_CollectionChanged2(object sender, EventArgs e)
        {
            ViewModel.ClearResult();
            ViewModel.Update();
        }

        private void Fleet_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ViewModel.ClearResult();
            ViewModel.Update();
            CalculateButton.IsEnabled = ViewModel.Attackers.Count > 0 && ViewModel.Defenders.Count > 0;
        }

        static readonly IFolder<CombatState, (int, int, int), (int, int, int)> folder = Folders.CountI<CombatState>().Combine(
                Folders.CountI<CombatState>(x => x.AttackerWinner == true),
                Folders.CountI<CombatState>(x => x.AttackerWinner == false),
                (count, attacker, defender) => (count, attacker, defender));

        private async void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (!double.IsFinite(SampleCountBox.Value))
            {
                return;
            }
            CalculateButton.IsEnabled = false;

            try
            {
                var attackerAi = AttackerFleet.SelectedAI.Implementation;
                var defenderAi = DefenderFleet.SelectedAI.Implementation;
                var amountToSample = (int)SampleCountBox.Value;
                int partitionCount = Math.Min(100, amountToSample);
                bool isPartitioned = amountToSample > 100;

                ViewModel.Progress = 0;
                ViewModel.ProgressMax = partitionCount;
                ViewModel.ProgressVisible = Visibility.Visible;

                var attackers = ViewModel.Attackers.Select(viewModel => (viewModel.Blueprint as IShipStats, viewModel.Count)).ToList();
                var defenders = ViewModel.Defenders.Select(viewModel => (viewModel.Blueprint as IShipStats, viewModel.Count)).ToList();

                int progress = 0;
                int completed = 0;
                void UpdateUI(CombatState state)
                {
                    completed++;
                    var newProgress = (int)((double)completed / amountToSample * 100);
                    if (progress != newProgress)
                    {
                        progress = newProgress;

                        this.DispatcherQueue.TryEnqueue(() =>
                        {
                            ViewModel.Progress = newProgress;
                        });
                    }
                }

                var states = await Task.Run(() => DoSampling(attackers, defenders, attackerAi, defenderAi, (int)amountToSample, UpdateUI));

                var (count, attacker, defender) = states.FoldWith(folder);

                ViewModel.AttackerWin += attacker;
                ViewModel.DefenderWin += defender;
                ViewModel.Combats += count;
                ViewModel.ProgressVisible = Visibility.Collapsed;
                ViewModel.Update();
            }
            finally
            {
                CalculateButton.IsEnabled = true;
            }
        }

        private static async Task<List<CombatState>> DoSampling(
            IEnumerable<(IShipStats blueprint, int count)> attackers,
            IEnumerable<(IShipStats blueprint, int count)> defenders,
            DamageAssigner attackerAi, DamageAssigner defenderAi,
            int amountToSample,
            Action<CombatState> callback)
        {
            async Task<IEnumerable<(ICombatShip, IEnumerable<DiceFace>)>> AssignDamage(
            ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<DiceFace> diceResult)
            {
                if (attacker.IsAttacker)
                {
                    return await attackerAi(attacker, targets, diceResult);
                }
                else
                {
                    return await defenderAi(attacker, targets, diceResult);
                }
            }

            Task<(int startRetreat, int completeRetreat)> RetreatAsker(ICombatShip activeShips)
            {
                return Task.FromResult((0, 0));
            }

            List<CombatState> states = [];
            for (int i = 0; i < amountToSample; i++)
            {
                await foreach (var state in CombatLogic.DoCombat(
                    attackers,
                    defenders,
                    AssignDamage, RetreatAsker))
                {
                    if (state.Ended)
                    {
                        callback(state);
                        states.Add(state);
                    }
                }
            }
            return states;
        }
    }
}
