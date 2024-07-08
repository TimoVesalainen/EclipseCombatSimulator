namespace EclipseCombatCalculatorLibrary
{
    public sealed class Damage : IDiceFace
    {
        public int DamageToOpponent { get; }
        public int DamageToSelf { get; }

        private Damage(int damageToOpponent, int damageToSelf = 0)
        {
            this.DamageToOpponent = damageToOpponent;
            this.DamageToSelf = damageToSelf;
        }

        public static Damage Create(int damageToOpponent, int damageToSelf = 0)
        {
            return new Damage(damageToOpponent, damageToSelf);
        }
    }
}
