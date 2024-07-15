using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EclipseCombatCalculator.Library.Dices;
using Nintenlord.Collections;
using Nintenlord.Collections.Comparers;
using Nintenlord.Distributions;

namespace EclipseCombatCalculator.Library
{
    public delegate Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> DamageAssigner(
        ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult);

    public delegate Task<(int startRetreat, int completeRetreat)> RetreatAsker(ICombatShip activeShips);

    public static class Combat
    {
        static readonly IComparer<CombatShip> initiativeComparer =
             Comparer<int>.Default.Select<int, CombatShip>(ship => ship.Blueprint.Initiative).Reverse().Then(
                 Comparer<bool>.Default.Select<bool, CombatShip>(ship => ship.IsAttacker).Reverse());

        private sealed class CombatShip : ICombatShip
        {
            public IShipStats Blueprint { get; }
            public bool IsAttacker { get; }

            public int InCombat { get; private set; }
            public int InRetreat { get; private set; }
            public int Retreated { get; private set; }
            public int Defeated { get; private set; }

            public int Damage { get; private set; }

            public CombatShip(IShipStats blueprint, int total, bool attacker)
            {
                Blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));
                InCombat = total;
                Damage = 0;
                IsAttacker = attacker;
            }

            public void AddDamage(int damage)
            {
                var newDamage = Damage + damage;
                while (newDamage > Blueprint.Hulls && InCombat + InRetreat > 0)
                {
                    newDamage -= Blueprint.Hulls + 1;
                    if (InCombat > 0)
                    {
                        // Prioritize destroying not retreating
                        InCombat--;
                    }
                    else
                    {
                        InRetreat--;
                    }
                    Defeated++;
                }
                Damage = newDamage;
            }

            public void HandleRetreat(int startRetreat, int completeRetreat)
            {
                InCombat -= startRetreat;
                InRetreat += startRetreat;
                InRetreat -= completeRetreat;
                Retreated += completeRetreat;
            }
        }

        public static async Task<bool> AttackerWin(
            IEnumerable<(IShipStats blueprint, int count)> attackers,
            IEnumerable<(IShipStats blueprint, int count)> defenders,
            DamageAssigner damageAssingment, RetreatAsker retreatAsker)
        {
            await foreach (var state in DoCombat(attackers, defenders, damageAssingment, retreatAsker))
            {
                if (state.Ended)
                {
                    return state.AttackerWinner.Value;
                }
            }
            throw new Exception("Combat ended without result");
        }

        public static async IAsyncEnumerable<CombatState> DoCombat(
            IEnumerable<(IShipStats blueprint, int count)> attackers,
            IEnumerable<(IShipStats blueprint, int count)> defenders,
            DamageAssigner damageAssingment, RetreatAsker retreatAsker)
        {
            var shipTypes = attackers.Select(shipType => new CombatShip(shipType.blueprint, shipType.count, attacker: true))
                .Concat(defenders.Select(shipType => new CombatShip(shipType.blueprint, shipType.count, attacker: false))).ToList();

            shipTypes.Sort(initiativeComparer);

            // Attack with either missiles or cannons
            async Task ActivateShips(IEnumerable<Dice> attackerDice, CombatShip attacker)
            {
                var distr = attackerDice
                    .Select(weaponDice => weaponDice.FaceDistribution.ArrayDistribution(attacker.InCombat))
                    .Distributions()
                    .Select(x => x.Flatten());

                var diceResults = distr.Sample();
                var targets = shipTypes.Where(target => target.IsAttacker != attacker.IsAttacker && target.InCombat > 0);

                var assignments = await damageAssingment(attacker, targets, diceResults);

                // TODO: Sanity checks?

                foreach (var (target, dices) in assignments)
                {
                    foreach (var dice in dices)
                    {
                        (target as CombatShip).AddDamage(dice.DamageToOpponent);
                    }
                }

                foreach (var diceResult in diceResults)
                {
                    attacker.AddDamage(diceResult.DamageToSelf);
                }
            }

            CombatState CommunicateCombatState(CombatStep step, CombatShip active)
            {
                var attackers = shipTypes.Where(type => type.IsAttacker);
                var defenders = shipTypes.Where(type => !type.IsAttacker);
                var attackersRemaining = attackers.Any(type => type.InCombat > 0 || type.InRetreat > 0);
                var defendersRemaining = defenders.Any(type => type.InCombat > 0 || type.InRetreat > 0);
                var isEnded = !attackersRemaining || !defendersRemaining;
                bool? attackerWin = isEnded ? attackersRemaining && !defendersRemaining : null;

                return new CombatState(
                    step,
                    shipTypes,
                    active,
                    attackers,
                    defenders,
                    attackerWin,
                    isEnded);
            }

            var missilesStartState = CommunicateCombatState(CombatStep.MissilesStart, null);
            yield return missilesStartState;

            // Fire missiles
            foreach (var attacker in shipTypes)
            {
                if (attacker.InCombat == 0)
                {
                    continue;
                }
                var state2 = CommunicateCombatState(CombatStep.MissileActivationStart, attacker);
                yield return state2;
                await ActivateShips(attacker.Blueprint.Missiles, attacker);
                var state = CommunicateCombatState(CombatStep.MissilesDamageApplied, attacker);
                yield return state;
                if (state.Ended)
                {
                    yield break;
                }
            }

            var cannonsStartState = CommunicateCombatState(CombatStep.CannonsStart, null);
            yield return cannonsStartState;

            // Fire cannons
            while (true)
            {
                foreach (var attacker in shipTypes)
                {
                    if (attacker.InCombat == 0)
                    {
                        continue;
                    }
                    var state2 = CommunicateCombatState(CombatStep.CannonActivationStart, attacker);
                    yield return state2;
                    var (startRetreat, completeRetreat) = await retreatAsker(attacker);
                    // TODO: Sanity checks?
                    attacker.HandleRetreat(startRetreat, completeRetreat);

                    var state1 = CommunicateCombatState(CombatStep.Retreat, attacker);
                    yield return state1;
                    if (state1.Ended)
                    {
                        yield break;
                    }

                    await ActivateShips(attacker.Blueprint.Cannons, attacker);
                    var state = CommunicateCombatState(CombatStep.CannonDamageApplied, attacker);
                    yield return state;
                    if (state.Ended)
                    {
                        yield break;
                    }
                }
            }
        }
    }
}
