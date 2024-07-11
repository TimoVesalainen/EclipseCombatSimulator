namespace EclipseCombatCalculator.Library.Dices
{
    public sealed class Miss : IDiceFace
    {
        public static readonly Miss Instance = new ();

        private Miss()
        {

        }

        public int DamageToOpponent => 0;

        public int DamageToSelf => 0;
    }
}
