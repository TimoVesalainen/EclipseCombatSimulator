namespace EclipseCombatCalculator.Library
{
    public interface ICombatShip
    {
        bool Attacker { get; }
        IShipStats Blueprint { get; }
        int Count { get; }
        int Damage { get; }
    }
}