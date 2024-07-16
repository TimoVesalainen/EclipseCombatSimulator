using EclipseCombatCalculator.Library;
using EclipseCombatCalculator.Library.Dices;
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
    public sealed class DiceAssignmentViewModel : INotifyPropertyChanged
    {
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
