using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library.Combat;
using System.Collections.ObjectModel;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class TargetShipViewModel : ViewModel
    {
        public ICombatShip Ship { get; set; }
        public Blueprint Blueprint { get; set; }
        public int Count { get; set; }
        public int Damage { get; set; }
        public ObservableCollection<DiceViewModel> AssignedDiceFaces { get; set; } = [];

        public static TargetShipViewModel Create(ICombatShip target)
        {
            return new TargetShipViewModel
            {
                Ship = target,
                Blueprint = target.Blueprint as Blueprint,
                Count = target.InCombat,
                Damage = target.Damage
            };
        }
    }
}
