namespace EclipseCombatCalculator.Library.Combat
{
    public interface ICombatShip
    {
        IShipStats Blueprint { get; }
        ShipCombatState State { get; }
        int Damage { get; }
    }

    public enum ShipCombatState
    {
        Combat,
        Retreating,
        Retreated,
        Destroyed,
    }
}