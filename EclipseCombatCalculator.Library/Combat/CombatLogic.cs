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
    public delegate Task<IEnumerable<(ICombatShip, IEnumerable<DiceFace>)>> DamageAssigner(
        IShipStats activeShipBlueprint, bool isAttacker, IEnumerable<ICombatShip> targets, IEnumerable<DiceFace> diceResult);

    public delegate Task<IEnumerable<(ICombatShip ship, ShipCombatState newState)>> RetreatAsker(bool attacker, IEnumerable<ICombatShip> ships);

    public static class CombatLogic
    {
        static readonly IComparer<ShipTypeCollection> initiativeComparer =
             Comparer<int>.Default.Select<int, ShipTypeCollection>(pair => pair.Blueprint.Initiative).Reverse().Then(
                 Comparer<bool>.Default.Select<bool, ShipTypeCollection>(ship => ship.Attacker).Reverse());

        private readonly struct ShipTypeCollection
        {
            public readonly IShipStats Blueprint;
            public readonly bool Attacker;
            public readonly CombatShip[] Ships;

            public ShipTypeCollection(IShipStats blueprint, bool attacker, int count)
            {
                Blueprint = blueprint;
                Attacker = attacker;
                Ships = Enumerable.Range(0, count).Select(_ => new CombatShip(blueprint)).ToArray();
            }

            public readonly bool AnyShipsInCombat()
            {
                return Ships.Where(ship => ship.State == ShipCombatState.Combat).Any();
            }

            public readonly bool AnyParticipating()
            {
                return Ships.Where(ship => ship.State == ShipCombatState.Combat && ship.State == ShipCombatState.Retreating).Any();
            }

            public readonly int InRetreat()
            {
                return Ships.Where(ship => ship.State == ShipCombatState.Retreating).Count();
            }

            public readonly int InCombat()
            {
                return Ships.Where(ship => ship.State == ShipCombatState.Combat).Count();
            }

            public readonly (IShipStats, bool isAttacker, IEnumerable<ICombatShip>) ToTuple()
            {
                return (Blueprint, Attacker, Ships);
            }
        }

        private sealed class CombatShip : ICombatShip
        {
            public IShipStats Blueprint { get; }
            public int Damage { get; private set; }
            public ShipCombatState State { get; set; } = ShipCombatState.Combat;

            public CombatShip(IShipStats blueprint, int startDamage = 0)
            {
                Blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));
                Damage = startDamage;
            }

            public void AddDamage(int damage)
            {
                Damage += damage;
                if (Damage > Blueprint.Hulls)
                {
                    State = ShipCombatState.Destroyed;
                }
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
            var shipTypes = attackers.Select(pair => new ShipTypeCollection(pair.blueprint, true, pair.count))
                .Concat(defenders.Select(pair => new ShipTypeCollection(pair.blueprint, false, pair.count))).ToList();

            shipTypes.Sort(initiativeComparer);
            List<DiceFace> dicesCache = new();

            List<CombatShip> attackerShips = shipTypes.Where(type => type.Attacker).SelectMany(type => type.Ships).ToList();
            List<CombatShip> defenderShips = shipTypes.Where(type => !type.Attacker).SelectMany(type => type.Ships).ToList();

            // Attack with either missiles or cannons
            async Task ActivateShips(IEnumerable<Dice> attackerDice, ShipTypeCollection attackers)
            {
                foreach (var attacker in attackers.Ships)
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

                var targets = shipTypes.Where(type => type.Attacker != attackers.Attacker)
                    .SelectMany(type => type.Ships.Where(ship => ship.State == ShipCombatState.Combat || ship.State == ShipCombatState.Retreating));

                var assignments = await damageAssignment(attackers.Blueprint, attackers.Attacker, targets, dicesCache);
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
                        if (attackers.Blueprint.CanHit(targetShip.Blueprint, dice))
                        {
                            targetShip.AddDamage(dice.DamageToOpponent);
                        }
#if DEBUG
                        usedDiceCount++;
#endif
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
                    // TODO: Assign damage according to AI
                    // attackers.AddDamage(diceResult.DamageToSelf);
                }
                dicesCache.Clear();
            }

            CombatState CommunicateCombatState(CombatStep step, ShipTypeCollection? active)
            {
                var attackersRemaining = attackerShips.Any(type => type.State == ShipCombatState.Combat || type.State == ShipCombatState.Retreating);
                var defendersRemaining = defenderShips.Any(type => type.State == ShipCombatState.Combat || type.State == ShipCombatState.Retreating);
                var isEnded = !attackersRemaining || !defendersRemaining;
                bool? attackerWin = isEnded ? attackersRemaining && !defendersRemaining : null;

                return new CombatState(
                    step,
                    shipTypes.Select(type => type.ToTuple()),
                    active?.Attacker,
                    active?.Blueprint,
                    active?.Ships,
                    attackerShips,
                    defenderShips,
                    attackerWin,
                    isEnded);
            }

            yield return CommunicateCombatState(CombatStep.MissilesStart, null);

            // Fire missiles
            foreach (var activeShipType in shipTypes)
            {
                if (activeShipType.AnyParticipating())
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
                    if (activeShipType.AnyParticipating())
                    {
                        continue;
                    }
                    yield return CommunicateCombatState(CombatStep.CannonActivationStart, activeShipType);
                    var retreatResult = await retreatAsker(activeShipType.Attacker,
                        activeShipType.Ships.Where(ship => ship.State == ShipCombatState.Combat || ship.State == ShipCombatState.Retreating));

                    foreach (var (ship, newState) in retreatResult)
                    {
#if DEBUG
                        if (newState == ShipCombatState.Destroyed)
                        {
                            throw new Exception("Cannot destroy ships when retreating");
                        }
                        else if (newState == ShipCombatState.Retreated && ship.State == ShipCombatState.Combat)
                        {
                            throw new Exception("Cannot skip straight to retreated");
                        }
#endif
                        (ship as CombatShip).State = newState;
                    }

                    var retreatState = CommunicateCombatState(CombatStep.Retreat, activeShipType);
                    yield return retreatState;
                    if (activeShipType.AnyParticipating())
                    {
                        continue;
                    }
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
