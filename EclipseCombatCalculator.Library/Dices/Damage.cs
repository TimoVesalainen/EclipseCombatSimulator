namespace EclipseCombatCalculator.Library.Dices
{
    public sealed class Damage : IDiceFace
    {
        public int DamageToOpponent { get; }
        public int DamageToSelf { get; }

        private Damage(int damageToOpponent, int damageToSelf = 0)
        {
            DamageToOpponent = damageToOpponent;
            DamageToSelf = damageToSelf;
        }

        public static Damage Create(int damageToOpponent, int damageToSelf = 0)
        {
            return new Damage(damageToOpponent, damageToSelf);
        }
    }
}
