using Nintenlord.Distributions;
using Nintenlord.Distributions.Discrete;

namespace EclipseCombatCalculatorLibrary
{
    public static class Dices
    {
        public readonly static Dice YellowDice = Dice.Create(Damage.Create(1), Number.Five, Number.Four, Number.Three, Number.Two, Miss.Instance);
        public readonly static Dice OrangeDice = Dice.Create(Damage.Create(2), Number.Five, Number.Four, Number.Three, Number.Two, Miss.Instance);
        public readonly static Dice BlueDice = Dice.Create(Damage.Create(3), Number.Five, Number.Four, Number.Three, Number.Two, Miss.Instance);
        public readonly static Dice RedDice = Dice.Create(Damage.Create(4), Number.Five, Number.Four, Number.Three, Number.Two, Miss.Instance);
        public readonly static Dice PurpleDice = Dice.Create(Damage.Create(3, 1), Damage.Create(2, 0), Damage.Create(1, 1), Damage.Create(0, 1), Miss.Instance, Miss.Instance);
    }
}
