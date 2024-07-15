using System;
using System.Collections.Generic;

namespace EclipseCombatCalculator.Library
{
    public readonly struct CombatState
    {
        public CombatStep CurrentStep { get; }
        public IEnumerable<ICombatShip> EngagementRoundOrder { get; }
        public ICombatShip Active { get; }
        public IEnumerable<ICombatShip> Attackers { get; }
        public IEnumerable<ICombatShip> Defenders { get; }
        public bool? AttackerWinner { get; }
        public bool Ended { get; }

        public CombatState(CombatStep currentStep, IEnumerable<ICombatShip> engagementRoundOrder, ICombatShip active,
            IEnumerable<ICombatShip> attackers, IEnumerable<ICombatShip> defenders, bool? attackerWinner, bool ended)
        {
            CurrentStep = currentStep;
            EngagementRoundOrder = engagementRoundOrder ?? throw new ArgumentNullException(nameof(engagementRoundOrder));
            Active = active;
            Attackers = attackers ?? throw new ArgumentNullException(nameof(attackers));
            Defenders = defenders ?? throw new ArgumentNullException(nameof(defenders));
            AttackerWinner = attackerWinner;
            Ended = ended;
        }
    }
}
