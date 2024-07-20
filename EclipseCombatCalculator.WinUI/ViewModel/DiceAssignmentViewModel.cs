using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class DiceAssignmentViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<DiceViewModel> AllDice { get; set; } = [];
        public ObservableCollection<DiceViewModel> UnAssignedFaces { get; set; } = [];
        public ObservableCollection<TargetShipViewModel> Ships { get; set; } = [];
        public CombatShipType AttackerShip { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
