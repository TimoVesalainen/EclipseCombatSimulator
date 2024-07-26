using System;
using System.Collections.Generic;
using EclipseCombatCalculator.Library.Dices;

namespace EclipseCombatCalculator.Library
{
    public interface IShipStats
    {
        public string Name { get; }
        public int Initiative { get; }
        public ShipType ShipType { get; }
        public IEnumerable<Dice> Cannons { get; }
        public IEnumerable<Dice> Missiles { get; }
        public int Computers { get; }
        public int Shields { get; }
        public int Hulls { get; }
        public int Size { get; }
    }

    public static class ShipStatHelpers
    {
        public static bool CanHit(this IShipStats attacker, IShipStats target, DiceFace result)
        {
            if (attacker is null)
            {
                throw new ArgumentNullException(nameof(attacker));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.Number == null)
            {
                return result.DamageToOpponent > 0;
            }

            return result.Number + attacker.Computers - target.Shields >= 6;
        }

        public static int DealtDamage(this IShipStats attacker, IShipStats target, DiceFace result)
        {
            if (attacker is null)
            {
                throw new ArgumentNullException(nameof(attacker));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.Number == null)
            {
                return result.DamageToOpponent;
            }

            return result.Number + attacker.Computers - target.Shields >= 6 ? result.DamageToOpponent : 0;

            throw new NotImplementedException("Not implemented");
        }

        public static bool MustHaveDrive(this IShipStats attacker)
        {
            return attacker.ShipType switch
            {
                ShipType.Interceptor or ShipType.Cruiser or ShipType.Dreadnaught => true,
                ShipType.Starbase => false,
                _ => throw new ArgumentOutOfRangeException(nameof(attacker)),
            };
        }

        public static bool CannotHaveDrive(this IShipStats attacker)
        {
            return attacker.ShipType switch
            {
                ShipType.Interceptor or ShipType.Cruiser or ShipType.Dreadnaught => false,
                ShipType.Starbase => true,
                _ => throw new ArgumentOutOfRangeException(nameof(attacker)),
            };
        }

        public static int CompareByShipType(this IShipStats ship1, IShipStats ship2)
        {
            // TODO: Starbase comparison might be wrong?

            if (ship1.ShipType == ship2.ShipType)
            {
                return 0;
            }
            else if (ship1.ShipType == ShipType.Interceptor)
            {
                return -1;
            }
            else if (ship1.ShipType == ShipType.Cruiser)
            {
                return ship2.ShipType == ShipType.Interceptor ? 1 : -1;
            }
            else if (ship1.ShipType == ShipType.Dreadnaught)
            {
                return ship2.ShipType == ShipType.Starbase ? -1 : 1;
            }
            else
            {
                return 1;
            }
        }
    }
}
