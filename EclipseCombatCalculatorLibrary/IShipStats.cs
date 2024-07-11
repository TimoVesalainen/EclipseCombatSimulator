using System;
using System.Collections.Generic;
using EclipseCombatCalculatorLibrary.Dices;

namespace EclipseCombatCalculatorLibrary
{
    public interface IShipStats
    {
        public int Initiative { get; }
        public IEnumerable<Dice> Cannons { get; }
        public IEnumerable<Dice> Missiles { get; }
        public int Computers { get; }
        public int Shields { get; }
        public int Hulls { get; }
        public int Size { get; }
    }

    public static class ShipStatHelpers
    {
        public static bool CanHit(this IShipStats attacker, IShipStats target, IDiceFace result)
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

            if (result is Damage)
            {
                return true;
            }
            if (result is Miss)
            {
                return false;
            }
            if (result is Number number)
            {
                return number.Value + attacker.Computers - target.Shields >= 6;
            }
            throw new NotImplementedException("Not implemented");
        }

        public static int DealtDamage(this IShipStats attacker, IShipStats target, IDiceFace result)
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

            if (result is Damage damage)
            {
                return damage.DamageToOpponent;
            }
            if (result is Miss)
            {
                return 0;
            }
            if (result is Number number)
            {
                return number.Value + attacker.Computers - target.Shields >= 6 ? number.DamageToOpponent : 0;
            }
            throw new NotImplementedException("Not implemented");
        }
    }
}
