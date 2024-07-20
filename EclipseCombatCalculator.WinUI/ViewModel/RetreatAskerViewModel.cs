using EclipseCombatCalculator.Library.Combat;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class RetreatAskerViewModel
    {
        public ICombatShip Ship { get; set; }

        public int StartRetreat { get; set; } = 0;
        public int CompleteRetreat { get; set; } = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
