namespace EclipseCombatCalculator.Library
{
    public interface ICombatShip
    {
        bool IsAttacker { get; } // TODO: Use other way to check who does assignment of damage
        IShipStats Blueprint { get; }

        int InCombat { get; }
        int InRetreat { get; }
        int Retreated { get; }
        int Defeated { get; }

        int Damage { get; }

    }
}