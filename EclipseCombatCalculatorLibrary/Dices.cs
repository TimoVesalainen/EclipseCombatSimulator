namespace EclipseCombatCalculatorLibrary
{
    public static class Dices
    {
        public readonly static Dice YellowDice = Dice.Create(Damage.Create(1), Number.Create(5, 1), Number.Create(4, 1), Number.Create(3, 1), Number.Create(2, 1), Miss.Instance);
        public readonly static Dice OrangeDice = Dice.Create(Damage.Create(2), Number.Create(5, 2), Number.Create(4, 2), Number.Create(3, 2), Number.Create(2, 2), Miss.Instance);
        public readonly static Dice BlueDice = Dice.Create(Damage.Create(3), Number.Create(5, 3), Number.Create(4, 3), Number.Create(3, 3), Number.Create(2, 3), Miss.Instance);
        public readonly static Dice RedDice = Dice.Create(Damage.Create(4), Number.Create(5, 4), Number.Create(4, 4), Number.Create(3, 4), Number.Create(2, 4), Miss.Instance);
        public readonly static Dice PurpleDice = Dice.Create(Damage.Create(3, 1), Damage.Create(2, 0), Damage.Create(1, 1), Damage.Create(0, 1), Miss.Instance, Miss.Instance);
    }
}
