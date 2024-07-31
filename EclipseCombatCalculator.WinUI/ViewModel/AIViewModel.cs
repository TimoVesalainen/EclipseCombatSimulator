using EclipseCombatCalculator.Library.Combat;
using System;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class AIViewModel(string name, DamageAssigner implementation) : ViewModel
    {
        public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));
        public DamageAssigner Implementation { get; } = implementation ?? throw new ArgumentNullException(nameof(implementation));
    }
}
