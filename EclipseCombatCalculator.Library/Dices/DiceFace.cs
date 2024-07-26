namespace EclipseCombatCalculator.Library.Dices
{
    public sealed class DiceFace
    {
        public Dice Dice { get; internal set; }
        public int FaceIndex { get; internal set; }

        public int DamageToOpponent { get; init; } = 0;

        public int DamageToSelf { get; init; } = 0;

        public int? Number { get; init; } = null;
    }
}
