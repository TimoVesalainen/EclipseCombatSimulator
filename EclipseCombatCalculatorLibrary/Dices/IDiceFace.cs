namespace EclipseCombatCalculatorLibrary.Dices
{
    public interface IDiceFace
    {
        int DamageToOpponent { get; }
        int DamageToSelf { get; }
    }
}
