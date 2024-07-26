namespace EclipseCombatCalculator.Library.Dices
{
    public static class CommonDices
    {
        public readonly static Dice YellowDice = Dice.CreateStandard(1);
        public readonly static Dice OrangeDice = Dice.CreateStandard(2);
        public readonly static Dice BlueDice = Dice.CreateStandard(3);
        public readonly static Dice RedDice = Dice.CreateStandard(4);
        public readonly static Dice PurpleDice = Dice.Create(
            new()
            {
                DamageToOpponent = 3,
                DamageToSelf = 1,
            },
            new()
            {
                DamageToOpponent = 2,
            },
            new()
            {
                DamageToOpponent = 1,
                DamageToSelf = 1,
            },
            new()
            {
                DamageToSelf = 1,
            },
            new(),
            new());
    }
}
