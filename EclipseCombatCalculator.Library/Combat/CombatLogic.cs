using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EclipseCombatCalculator.Library.Dices;
using Nintenlord.Collections;
using Nintenlord.Collections.Comparers;
using Nintenlord.Distributions;

namespace EclipseCombatCalculator.Library.Combat
{
    // TODO: Make support assigning damage per ship, not ship type
    public delegate Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> DamageAssigner(
        ICombatShip activeShips, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult);

    public delegate Task<(int startRetreat, int completeRetreat)> RetreatAsker(ICombatShip activeShips);

    public static class CombatLogic
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
                InCombat += InRetreat - completeRetreat;
                InRetreat = startRetreat;
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
            DamageAssigner damageAssignment, RetreatAsker retreatAsker)
        {
            var shipTypes = attackers.Select(shipType => new CombatShip(shipType.blueprint, shipType.count, attacker: true))
                .Concat(defenders.Select(shipType => new CombatShip(shipType.blueprint, shipType.count, attacker: false))).ToList();

            shipTypes.Sort(initiativeComparer);
            List<IDiceFace> dicesCache = new();

            // Attack with either missiles or cannons
            async Task ActivateShips(IEnumerable<Dice> attackerDice, CombatShip attacker)
            {
                for (int i = 0; i < attacker.InCombat; i++)
                {
                    dicesCache.AddRange(attackerDice.Select(x => x.FaceDistribution.Sample()));
                }
                /*
                TODO: Use this once this is performant
                var distr = attackerDice
                    .Select(weaponDice => weaponDice.FaceDistribution.RepeatedDistribution(attacker.InCombat))
                    .Distributions()
                    .Select(x => x.Flatten());

                var diceResults = distr.Sample();*/
                var targets = shipTypes.Where(target => target.IsAttacker != attacker.IsAttacker && target.InCombat + target.InRetreat > 0);

                var assignments = await damageAssignment(attacker, targets, dicesCache);
#if DEBUG
                int usedDiceCount = 0;
#endif
                foreach (var (target, dices) in assignments)
                {
                    var targetShip = target as CombatShip;
#if DEBUG
                    if (!targets.Contains(targetShip))
                    {
                        throw new Exception("Targetting invalid ship");
                    }
#endif
                    foreach (var dice in dices)
                    {
#if DEBUG
                        if (!dicesCache.Contains(dice))
                        {
                            throw new Exception("Attempting to cheat by creating new dice");
                        }
#endif
                        if (attacker.Blueprint.CanHit(targetShip.Blueprint, dice))
                        {
                            targetShip.AddDamage(dice.DamageToOpponent);
                        }
                        usedDiceCount++;
                    }
                }
#if DEBUG
                if (usedDiceCount > dicesCache.Count)
                {
                    throw new Exception("Attempting to cheat by creating or re-using dice");
                }
#endif

                foreach (var diceResult in dicesCache)
                {
                    attacker.AddDamage(diceResult.DamageToSelf);
                }
                dicesCache.Clear();
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

            yield return CommunicateCombatState(CombatStep.MissilesStart, null);

            // Fire missiles
            foreach (var activeShipType in shipTypes)
            {
                if (activeShipType.InCombat == 0)
                {
                    continue;
                }
                yield return CommunicateCombatState(CombatStep.MissileActivationStart, activeShipType);
                await ActivateShips(activeShipType.Blueprint.Missiles, activeShipType);
                var state = CommunicateCombatState(CombatStep.MissilesDamageApplied, activeShipType);
                yield return state;
                if (state.Ended)
                {
                    yield break;
                }
            }

            yield return CommunicateCombatState(CombatStep.CannonsStart, null);

            // Fire cannons
            while (true)
            {
                foreach (var activeShipType in shipTypes)
                {
                    if (activeShipType.InCombat == 0 && activeShipType.InRetreat == 0)
                    {
                        continue;
                    }
                    yield return CommunicateCombatState(CombatStep.CannonActivationStart, activeShipType);
                    var (startRetreat, completeRetreat) = await retreatAsker(activeShipType);
#if DEBUG
                    if (startRetreat < 0 || completeRetreat < 0)
                    {
                        throw new Exception("Negative value returned from callback");
                    }
                    if (startRetreat > activeShipType.InCombat)
                    {
                        throw new Exception("Cannot retreat more ships that are in combat");
                    }
                    if (completeRetreat > activeShipType.InRetreat)
                    {
                        throw new Exception("Cannot complete retreat more ships that are in retreat");
                    }
#endif
                    activeShipType.HandleRetreat(startRetreat, completeRetreat);

                    var retreatState = CommunicateCombatState(CombatStep.Retreat, activeShipType);
                    yield return retreatState;
                    if (retreatState.Ended)
                    {
                        yield break;
                    }

                    await ActivateShips(activeShipType.Blueprint.Cannons, activeShipType);
                    var attackState = CommunicateCombatState(CombatStep.CannonDamageApplied, activeShipType);
                    yield return attackState;
                    if (attackState.Ended)
                    {
                        yield break;
                    }
                    // TODO: Implement Morph shield
                }
            }
        }
    }
}
