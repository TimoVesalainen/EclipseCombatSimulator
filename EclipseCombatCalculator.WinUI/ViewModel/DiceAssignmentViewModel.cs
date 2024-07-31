using System.Collections.ObjectModel;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class DiceAssignmentViewModel : ViewModel
    {
        public ObservableCollection<DiceViewModel> AllDice { get; set; } = [];
        public ObservableCollection<DiceViewModel> UnAssignedFaces { get; set; } = [];
        public ObservableCollection<TargetShipViewModel> Ships { get; set; } = [];
        public CombatShipType AttackerShip { get; set; }
    }
}
