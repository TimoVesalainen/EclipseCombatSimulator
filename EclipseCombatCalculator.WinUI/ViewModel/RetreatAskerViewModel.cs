using EclipseCombatCalculator.Library.Combat;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class RetreatAskerViewModel : ViewModel
    {
        public ICombatShip Ship { get; set; }

        public int StartRetreat { get; set; } = 0;
        public int CompleteRetreat { get; set; } = 0;
    }
}
