using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class CombatPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CombatShipType> Attackers { get; } = [CombatShipType.Create(Blueprint.TerranInterceptor)];
        public ObservableCollection<CombatShipType> Defenders { get; } = [CombatShipType.Create(Blueprint.OrionCruiser)];
        public ObservableCollection<AIViewModel> AIs { get; } = [new AIViewModel("Basic", AI.BasicAI)];
        public bool CanStartCombat => Attackers.Count > 0 && Defenders.Count > 0;

        public void Remove(CombatShipType ship)
        {
            Attackers.Remove(ship);
            Defenders.Remove(ship);
            NotifyPropertyChanged(nameof(CanStartCombat));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
