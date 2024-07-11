using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EclipseCombatCalculatorLibrary.Dices;
using Nintenlord.Collections;
using Nintenlord.Collections.Comparers;
using Nintenlord.Distributions;

namespace EclipseCombatCalculatorLibrary
{
    public delegate Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> DamageAssigner(
        ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult);

    public static class Combat
    {
        static readonly IComparer<CombatShip> initiativeComparer =
             Comparer<int>.Default.Select<int, CombatShip>(ship => ship.Blueprint.Initiative).Reverse().Then(
                 Comparer<bool>.Default.Select<bool, CombatShip>(ship => ship.Attacker).Reverse());

        private sealed class CombatShip : ICombatShip
        {
            public IShipStats Blueprint { get; }
            public bool Attacker { get; }
            public int Count { get; private set; }
            public int Damage { get; private set; }

            public CombatShip(IShipStats blueprint, int count, bool attacker)
            {
                Blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));
                Count = count;
                Damage = 0;
                Attacker = attacker;
            }

            public void AddDamage(int damage)
            {
                var newDamage = Damage + damage;

                while (newDamage > Blueprint.Hulls)
                {
                    Count--;
                    newDamage -= Blueprint.Hulls + 1;
                }
                Damage = newDamage;
            }
        }

        public static async Task<bool> AttackerWin(
            IEnumerable<(IShipStats blueprint, int count)> attackers,
            IEnumerable<(IShipStats blueprint, int count)> defenders,
            DamageAssigner damageAssingment)
        {
            var shipTypes = attackers.Select(shipType => new CombatShip(shipType.blueprint, shipType.count, attacker: true))
                .Concat(defenders.Select(shipType => new CombatShip(shipType.blueprint, shipType.count, attacker: false))).ToList();

            shipTypes.Sort(initiativeComparer);

            async Task<bool?> CombatRound(IEnumerable<Dice> attackerDice, CombatShip attacker)
            {
                var distr = attacker.Blueprint.Cannons.Select(weaponDice => weaponDice.FaceDistribution.ArrayDistribution(attacker.Count))
                    .Distributions().Select(x => x.Flatten());

                var diceResults = distr.Sample();
                var targets = shipTypes.Where(target => target.Attacker != attacker.Attacker && target.Count > 0);

                var assignments = await damageAssingment(attacker, targets, diceResults);

                // TODO: Sanity checks?

                foreach (var (target, dices) in assignments)
                {
                    foreach (var dice in dices)
                    {
                        if (attacker.Blueprint.CanHit(target.Blueprint, dice) && target.Count > 0)
                        {
                            (target as CombatShip).AddDamage(dice.DamageToOpponent);
                        }
                    }
                }

                foreach (var diceResult in diceResults)
                {
                    attacker.AddDamage(diceResult.DamageToSelf);
                }

                // TODO: Mutual KO result
                if (!shipTypes.Any(type => type.Attacker != attacker.Attacker && type.Count > 0))
                {
                    return attacker.Attacker;
                }
                return null;
            }

            // Fire missiles
            foreach (var attacker in shipTypes)
            {
                var result = await CombatRound(attacker.Blueprint.Missiles, attacker);
                if (result is bool endResult)
                {
                    return endResult;
                }
            }

            // Fire cannons
            while (true)
            {
                foreach (var attacker in shipTypes)
                {
                    // TODO: Check if wants to try to retreat, or complete retreat
                    var result = await CombatRound(attacker.Blueprint.Cannons, attacker);
                    if (result is bool endResult)
                    {
                        return endResult;
                    }
                }
            }
        }
    }
}
