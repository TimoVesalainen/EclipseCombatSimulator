using EclipseCombatCalculator.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class AIViewModel(string name, DamageAssigner implementation)
    {
        public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));
        public DamageAssigner Implementation { get; } = implementation ?? throw new ArgumentNullException(nameof(implementation));
    }
}
