using System;
using System.Collections.Generic;
using System.Linq;

namespace EclipseCombatCalculator.Library.Combat
{
    public readonly struct CombatState
    {
        public CombatStep CurrentStep { get; }
        public IEnumerable<(IShipTypeStats, bool isAttacker, IEnumerable<ICombatShip>)> EngagementRoundOrder { get; }
        public bool? IsActiveAttacker { get; }
        public IShipTypeStats ActiveShipBlueprint { get; }
        public IEnumerable<ICombatShip> ActiveShips { get; }
        public IEnumerable<ICombatShip> Attackers { get; }
        public IEnumerable<ICombatShip> Defenders { get; }
        public bool? AttackerWinner { get; }
        public bool Ended { get; }

        public CombatState(CombatStep currentStep, IEnumerable<(IShipTypeStats, bool isAttacker, IEnumerable<ICombatShip>)> engagementRoundOrder,
            bool? isActiveAttacker, IShipTypeStats activeShipBlueprint, IEnumerable<ICombatShip> activeShips,
            IEnumerable<ICombatShip> attackers, IEnumerable<ICombatShip> defenders,
            bool? attackerWinner, bool ended)
        {
            CurrentStep = currentStep;
            EngagementRoundOrder = engagementRoundOrder ?? throw new ArgumentNullException(nameof(engagementRoundOrder));
            IsActiveAttacker = isActiveAttacker;
            ActiveShipBlueprint = activeShipBlueprint;
            ActiveShips = activeShips ?? Enumerable.Empty<ICombatShip>();
            Attackers = attackers ?? throw new ArgumentNullException(nameof(attackers));
            Defenders = defenders ?? throw new ArgumentNullException(nameof(defenders));
            AttackerWinner = attackerWinner;
            Ended = ended;
        }
    }
}
