using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library.Combat;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class CalculationsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CombatShipType> Attackers { get; } = [CombatShipType.Create(Blueprint.TerranInterceptor)];
        public ObservableCollection<CombatShipType> Defenders { get; } = [CombatShipType.Create(Blueprint.OrionCruiser)];
        public ObservableCollection<AIViewModel> AIs { get; } = [new AIViewModel("Basic", AI.BasicAI)];
        public bool CanStartCombat => Attackers.Count > 0 && Defenders.Count > 0;

        private int attackerWin;
        public int AttackerWin
        {
            get => attackerWin;
            set
            {
                attackerWin = value;
                NotifyPropertyChanged();
            }
        }
        private int defenderWin;
        public int DefenderWin
        {
            get => defenderWin;
            set
            {
                defenderWin = value;
                NotifyPropertyChanged();
            }
        }
        private int combats;
        public int Combats
        {
            get => combats;
            set
            {
                combats = value;
                NotifyPropertyChanged();
            }
        }

        private string result;
        public string Result
        {
            get => result;
            set
            {
                result = value;
                NotifyPropertyChanged();
            }
        }

        private int progress;
        public int Progress
        {
            get => progress;
            set
            {
                progress = value;
                NotifyPropertyChanged();
            }
        }

        private int progressMax;
        public int ProgressMax
        {
            get => progressMax;
            set
            {
                progressMax = value;
                NotifyPropertyChanged();
            }
        }

        public Visibility progressVisible = Visibility.Collapsed;
        public Visibility ProgressVisible
        {
            get => progressVisible;
            set
            {
                progressVisible = value;
                NotifyPropertyChanged();
            }
        }

        public CalculationsViewModel()
        {
            Attackers.CollectionChanged += Ships_CollectionChanged;
            Defenders.CollectionChanged += Ships_CollectionChanged;
        }

        public void Update()
        {
            var attackerWinPercentage = (double)attackerWin * 100 / combats;
            var defenderWinPercentage = (double)defenderWin * 100 / combats;

            Result = $"Of {combats} samples:\n" +
                $"Attacker {attackerWin}, Defender {defenderWin}\n" +
                $"Attacker win portion: {attackerWinPercentage}%\n" +
                $"Defender win portion: {defenderWinPercentage}%\n";
        }

        private void Ships_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((sender as ObservableCollection<CombatShipType>).Count == e.NewItems.Count)
                    {
                        NotifyPropertyChanged(nameof(CanStartCombat));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if ((sender as ObservableCollection<CombatShipType>).Count == 0)
                    {
                        NotifyPropertyChanged(nameof(CanStartCombat));
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    NotifyPropertyChanged(nameof(CanStartCombat));
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        public void Remove(CombatShipType ship)
        {
            Attackers.Remove(ship);
            Defenders.Remove(ship);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
