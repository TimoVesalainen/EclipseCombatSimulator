using EclipseCombatCalculatorLibrary.Blueprints;
using EclipseCombatCalculatorLibrary.Dices;
using Nintenlord.Collections.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculatorLibrary
{
    public static class AI
    {
        static readonly IComparer<ICombatShip> BySizeComparer = Comparer<int>.Default.Reverse()
            .Select<int, ICombatShip>(shipType => shipType.Blueprint.Size);

        static readonly IComparer<IDiceFace> DiceResultSorter = Comparer<int>.Default.Select<int, IDiceFace>(face => face is Damage ? 0 : (face as Number)?.Value ?? 10);

        public static readonly DamageAssigner BasicAI = async (attacker, targets, diceResult) =>
        {
            var attackerComputer = attacker.Blueprint.Computers;

            var dices = diceResult.ToList();
            dices.Sort(DiceResultSorter);

            var targetsList = targets.ToList();
            targetsList.Sort(BySizeComparer);

            List<(ICombatShip, IEnumerable<IDiceFace>)> assigned = new(targetsList.Count);

            foreach (var target in targetsList)
            {
                if (dices.Count == 0)
                {
                    break;
                }
                List<IDiceFace> assignedDice = new();
                int remainingHealth = target.Count * (target.Blueprint.Hulls + 1) - target.Damage;
                foreach (var dice in dices)
                {
                    if (attacker.Blueprint.CanHit(target.Blueprint, dice))
                    {
                        assignedDice.Add(dice);
                        remainingHealth -= dice.DamageToOpponent;
                        if (remainingHealth <= 0)
                        {
                            break;
                        }
                    }
                }
                foreach (var dice in assignedDice)
                {
                    dices.Remove(dice);
                }
                if (assignedDice.Count > 0)
                {
                    assigned.Add((target, assignedDice.AsEnumerable()));
                }
            }

            return assigned;
        };

        // TODO: Implement AI as in manual:
        /*
         Hits and damage with non-player opponents
        When you battle non-player opponents, any other player rolls
        the dice for Ancient, Guardian, and GCDS Attacks. If possible,
        dice are assigned so that your Ships are destroyed from largest to smallest.
        If none of your Ships can be destroyed in the Attack,
        dice are assigned to inflict as much damage to your Ships as possible,
        from largest to smallest.
         */
    }
}
