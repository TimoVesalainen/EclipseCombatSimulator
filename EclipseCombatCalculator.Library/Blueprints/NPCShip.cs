using EclipseCombatCalculator.Library.Dices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculator.Library.Blueprints
{
    public sealed class NPCShip : IShipTypeStats
    {
        public string Name { get; init; }
        public ShipType ShipType { get; init; }
        public int Initiative { get; init; } = 0;

        public IEnumerable<Dice> Cannons { get; init; } = Enumerable.Empty<Dice>();
        public IEnumerable<Dice> Missiles { get; init; } = Enumerable.Empty<Dice>();

        public int Computers { get; init; } = 0;
        public int Shields { get; init; } = 0;
        public int Hulls { get; init; } = 0;
        public int Size { get; init; } = 0;

        static readonly List<NPCShip> ships = new();

        private NPCShip()
        {
            ships.Add(this);
        }

        public static IEnumerable<NPCShip> Ships => ships;

        // Ancients
        public static readonly NPCShip EasyAncient = new()
        {
            Name = "Ancient",
            ShipType = ShipType.Ancient,
            Cannons = new[] { CommonDices.YellowDice, CommonDices.YellowDice },
            Computers = 1,
            Hulls = 1,
            Initiative = 2,
        };
        public static readonly NPCShip HardAncient = new()
        {
            Name = "Hard Ancient",
            ShipType = ShipType.Ancient,
            Cannons = new[] { CommonDices.OrangeDice },
            Computers = 1,
            Hulls = 2,
            Initiative = 1,
        };
        public static readonly NPCShip HardAncientEx = new()
        {
            Name = "Hard Ancient Ex",
            ShipType = ShipType.Ancient,
            Cannons = new[] { CommonDices.OrangeDice },
            Computers = 2,
            Hulls = 1,
            Initiative = 3,
        };

        // Guardians
        public static readonly NPCShip EasyGuardian = new()
        {
            Name = "Guardian",
            ShipType = ShipType.Guardian,
            Cannons = new[] { CommonDices.YellowDice, CommonDices.YellowDice, CommonDices.YellowDice },
            Computers = 2,
            Hulls = 2,
            Initiative = 3,
        };
        public static readonly NPCShip HardGuardian = new()
        {
            Name = "Hard Guardian",
            ShipType = ShipType.Guardian,
            Cannons = new[] { CommonDices.RedDice },
            Missiles = new[] { CommonDices.OrangeDice, CommonDices.OrangeDice },
            Computers = 1,
            Hulls = 3,
            Initiative = 1,
        };
        public static readonly NPCShip HardGuardianEx = new()
        {
            Name = "Hard Guardian Ex",
            ShipType = ShipType.Guardian,
            Cannons = new[] { CommonDices.OrangeDice, CommonDices.OrangeDice },
            Computers = 1,
            Shields = 1,
            Hulls = 3,
            Initiative = 2,
        };

        // GCDSs
        public static readonly NPCShip EasyGCDS = new()
        {
            Name = "GCDS",
            ShipType = ShipType.GalacticCenterDefenseSystem,
            Cannons = new[] { CommonDices.YellowDice, CommonDices.YellowDice, CommonDices.YellowDice, CommonDices.YellowDice },
            Computers = 2,
            Hulls = 7,
        };
        public static readonly NPCShip HardGCDS = new()
        {
            Name = "Hard GCDSs",
            ShipType = ShipType.GalacticCenterDefenseSystem,
            Cannons = new[] { CommonDices.RedDice },
            Missiles = new[] { CommonDices.YellowDice, CommonDices.YellowDice, CommonDices.YellowDice, CommonDices.YellowDice },
            Computers = 2,
            Hulls = 3,
            Initiative = 2,
        };
        public static readonly NPCShip HardGCDSEx = new()
        {
            Name = "Hard GCDSs Ex",
            ShipType = ShipType.GalacticCenterDefenseSystem,
            Cannons = new[] { CommonDices.OrangeDice, CommonDices.OrangeDice },
            Computers = 2,
            Shields = 2,
            Hulls = 4,
            Initiative = 3,
        };
    }
}
