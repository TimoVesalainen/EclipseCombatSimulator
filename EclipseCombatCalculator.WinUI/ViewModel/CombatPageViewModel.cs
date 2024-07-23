using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library.Combat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class CombatPageViewModel
    {
        public ObservableCollection<CombatShipType> Attackers { get; } = [CombatShipType.Create(Blueprint.TerranInterceptor)];
        public ObservableCollection<CombatShipType> Defenders { get; } = [CombatShipType.Create(Blueprint.OrionCruiser)];
        public ObservableCollection<AIViewModel> AIs { get; } = [new AIViewModel("Basic", AI.BasicAI)];
    }
}
