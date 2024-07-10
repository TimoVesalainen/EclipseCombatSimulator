namespace EclipseCombatCalculatorLibrary.Dices
{
    public static class CommonDices
    {
        public readonly static Dice YellowDice = Dice.CreateStandard(1);
        public readonly static Dice OrangeDice = Dice.CreateStandard(2);
        public readonly static Dice BlueDice = Dice.CreateStandard(3);
        public readonly static Dice RedDice = Dice.CreateStandard(4);
        public readonly static Dice PurpleDice = Dice.Create(Damage.Create(3, 1), Damage.Create(2, 0), Damage.Create(1, 1), Damage.Create(0, 1), Miss.Instance, Miss.Instance);
    }
}
