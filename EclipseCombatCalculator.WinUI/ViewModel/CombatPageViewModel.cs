using EclipseCombatCalculator.Library.Blueprints;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using EclipseCombatCalculator.Library.Combat;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class CombatPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CombatShipType> Attackers { get; } = [CombatShipType.Create(Blueprint.TerranInterceptor)];
        public ObservableCollection<CombatShipType> Defenders { get; } = [CombatShipType.Create(Blueprint.OrionCruiser)];
        public ObservableCollection<AIViewModel> AIs { get; } = [new AIViewModel("Basic", AI.BasicAI)];
        public bool CanStartCombat => Attackers.Count > 0 && Defenders.Count > 0;

        public CombatPageViewModel()
        {
            Attackers.CollectionChanged += Attackers_CollectionChanged;
            Defenders.CollectionChanged += Attackers_CollectionChanged;
        }

        private void Attackers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
            NotifyPropertyChanged(nameof(CanStartCombat));
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
