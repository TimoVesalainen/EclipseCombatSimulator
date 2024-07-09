using System;
using System.Collections.Generic;
using System.Linq;
using Nintenlord.Collections;
using Nintenlord.Collections.Comparers;
using Nintenlord.Distributions;

namespace EclipseCombatCalculatorLibrary
{
    public static class Combat
    {
        static readonly Comparison<(IShipStats blueprint, int count)> initiativeComparer = (x, y) => Comparer<int>.Default.Compare(y.blueprint.Initiative, x.blueprint.Initiative);

        private sealed class CombatShip
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
                    newDamage = 0;
                }
            }
        }

        public static bool AttackerWin(IEnumerable<(IShipStats blueprint, int count)> attackers, IEnumerable<(IShipStats blueprint, int count)> defenders)
        {
            (IShipStats blueprint, int count)[] attackersArray = attackers.ToArray();
            (IShipStats blueprint, int count)[] defendersArray = defenders.ToArray();

            Array.Sort(attackersArray, initiativeComparer);
            Array.Sort(defendersArray, initiativeComparer);

            var shipTypes = attackersArray.Select(shipType => new CombatShip(shipType.blueprint, shipType.count, attacker: true))
                .Concat(defendersArray.Select(shipType => new CombatShip(shipType.blueprint, shipType.count, attacker: false))).ToList();

            while (true)
            {
                foreach (var attacker in shipTypes)
                {
                    // TODO: Check if wants to try to retreat, or complete retreat

                    var distr = attacker.Blueprint.Weapons.Select(weaponDice => weaponDice.FaceDistribution.ArrayDistribution(attacker.Count))
                        .Distributions().Select(x => x.Flatten());

                    var diceResults = distr.Sample();

                    // TODO: Assign damage smartly
                    // TODO: Damage splitting
                    foreach (var diceResult in diceResults)
                    {
                        attacker.AddDamage(diceResult.DamageToSelf);

                        if (diceResult is Miss)
                        {
                            continue;
                        }

                        foreach (var defender in shipTypes.Where(type => type.Attacker != attacker.Attacker).Reverse())
                        {
                            if (attacker.Blueprint.CanHit(defender.Blueprint, diceResult) && defender.Count > 0)
                            {
                                defender.AddDamage(diceResult.DamageToOpponent);
                                break;
                            }
                        }
                    }

                    // TODO: Mutual KO result
                    if (!shipTypes.Any(type => type.Attacker != attacker.Attacker && type.Count > 0))
                    {
                        return attacker.Attacker;
                    }
                }
            }
        }
    }
}
