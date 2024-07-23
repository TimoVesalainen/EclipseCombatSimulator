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

        public void Update()
        {
            if (combats == 0)
            {
                Result = "";
                return;
            }

            var attackerWinPercentage = (double)attackerWin * 100 / combats;
            var defenderWinPercentage = (double)defenderWin * 100 / combats;

            Result = $"Of {combats} samples:\n" +
                $"Attacker {attackerWin}, Defender {defenderWin}\n" +
                $"Attacker win portion: {attackerWinPercentage:F2}%\n" +
                $"Defender win portion: {defenderWinPercentage:F2}%\n";
        }

        public void ClearResult()
        {
            attackerWin = 0;
            defenderWin = 0;
            combats = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
