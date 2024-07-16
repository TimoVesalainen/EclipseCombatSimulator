using EclipseCombatCalculator.Library;
using EclipseCombatCalculator.Library.Blueprints;
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
    public sealed class TargetShipViewModel : INotifyPropertyChanged
    {
        public Blueprint Blueprint { get; set; }
        public int Count { get; set; }
        public int Damage { get; set; }
        public ObservableCollection<IDiceFace> AssignedDiceFaces { get; set; } = [];

        public static TargetShipViewModel Create(ICombatShip target)
        {
            return new TargetShipViewModel
            {
                Blueprint = target.Blueprint as Blueprint,
                Count = target.InCombat,
                Damage = target.Damage
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
