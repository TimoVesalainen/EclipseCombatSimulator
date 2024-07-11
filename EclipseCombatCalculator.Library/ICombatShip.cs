namespace EclipseCombatCalculator.Library
{
    public interface ICombatShip
    {
        bool Attacker { get; } // TODO: Use other way to check who does assignment of damage
        IShipStats Blueprint { get; }
        int Count { get; }
        int Damage { get; }
    }
}