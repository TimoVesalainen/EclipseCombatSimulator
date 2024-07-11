namespace EclipseCombatCalculator.Library.Dices
{
    public sealed class Number : IDiceFace
    {
        public int Value { get; }

        public int DamageToOpponent { get; }

        public int DamageToSelf { get; }

        private Number(int value, int damage, int damageToSelf)
        {
            Value = value;
            DamageToOpponent = damage;
            DamageToSelf = damageToSelf;
        }

        public static Number Create(int value, int damage, int damageToSelf = 0)
        {
            return new Number(value, damage, damageToSelf);
        }
    }
}
