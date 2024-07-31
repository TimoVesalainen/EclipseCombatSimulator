using System.Collections.ObjectModel;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class DiceAssignmentViewModel : ViewModel
    {
        public ObservableCollection<DiceViewModel> AllDice { get; } = [];
        public ObservableCollection<DiceViewModel> UnAssignedFaces { get; } = [];
        public ObservableCollection<TargetShipViewModel> Ships { get; } = [];
        public CombatShipType AttackerShip
        {
            get => attackerShip;
            set
            {
                if (attackerShip == value) return;
                attackerShip = value;
                NotifyPropertyChanged();
            }
        }

        private CombatShipType attackerShip;
    }
}
