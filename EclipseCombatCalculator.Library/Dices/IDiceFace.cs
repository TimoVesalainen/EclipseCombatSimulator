namespace EclipseCombatCalculator.Library.Dices
{
    public interface IDiceFace
    {
        int DamageToOpponent { get; }
        int DamageToSelf { get; }
    }
}
