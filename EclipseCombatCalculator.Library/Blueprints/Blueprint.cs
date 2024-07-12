﻿using EclipseCombatCalculator.Library.Dices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EclipseCombatCalculator.Library.Blueprints
{
    public sealed class Blueprint : IShipStats
    {
        public string Name { get; init; }
        public int BaseInitiative { get; init; } = 0;
        public int BaseComputer { get; init; } = 0;
        public int BaseShield { get; init; } = 0;
        public int BaseEnergy { get; init; } = 0;
        public int BaseHull { get; init; } = 0;

        public bool IsBase { get; init; } = false;

        public Part this[int index]
        {
            get => slots[index];
            set {
                if (readOnlyBlueprint)
                {
                    throw new InvalidOperationException("Trying to edit read only Blueprint");
                }
                slots[index] = value;
            }
        }

        public int Size => slots.Length;

        readonly Part[] slots;
        readonly bool readOnlyBlueprint;

        IEnumerable<Part> Parts => slots.Where(x => x != null);

        readonly static List<Blueprint> blueprints = new();

        public static IEnumerable<Blueprint> Blueprints => blueprints;

        private Blueprint(string name, params Part[] parts)
        {
            Name = name;
            this.slots = parts;
            readOnlyBlueprint = true;
            blueprints.Add(this);
        }
        private Blueprint(Blueprint other, bool readOnlyBlueprint)
        {
            Name = other.Name;
            this.BaseInitiative = other.BaseInitiative;
            this.BaseComputer = other.BaseComputer;
            this.BaseShield = other.BaseShield;
            this.BaseEnergy = other.BaseEnergy;
            this.BaseHull = other.BaseHull;
            this.IsBase = other.IsBase;
            this.slots = other.slots.ToArray(); // Shallow clone
            this.readOnlyBlueprint = readOnlyBlueprint;
            blueprints.Add(this);
        }

        public Blueprint CreateEditableClone()
        {
            return new Blueprint(this, false);
        }

        private Blueprint Clone(string name)
        {
            return new Blueprint(this, true)
            {
                Name = name
            };
        }

        public int TotalEnergy => BaseEnergy + Parts.Sum(part => part.Energy);

        int IShipStats.Initiative => BaseInitiative + Parts.Sum(part => part.Initiative);

        int IShipStats.Computers => BaseComputer + Parts.Sum(part => part.Computers);

        int IShipStats.Shields => BaseShield + Parts.Sum(part => part.Shields);

        int IShipStats.Hulls => BaseHull + Parts.Sum(part => part.Hulls);

        IEnumerable<Dice> IShipStats.Cannons => Parts.SelectMany(part => part.Cannons);

        IEnumerable<Dice> IShipStats.Missiles => Parts.SelectMany(part => part.Missiles);

        //Default aka Terran, Mechanema, Hydran, Draco, Magellan, Lyra
        public static readonly Blueprint TerranInterceptor = new("Terran Interceptor",
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint TerranCruiser = new("Terran Cruiser",
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint TerranDreadnaught = new("Terran Dreadnaught",
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint TerranStarbase = new("Terran Starbase",
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        // TODO: Each race gets it's own blueprints

        //Eridani
        public static readonly Blueprint EridaniInterceptor = new("Eridani Interceptor",
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2, BaseEnergy = 1 };
        public static readonly Blueprint EridaniCruiser = new("Eridani Cruiser",
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1, BaseEnergy = 1 };
        public static readonly Blueprint EridaniDreadnaught = new("Eridani Dreadnaught",
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive)
        { BaseEnergy = 1 };
        public static readonly Blueprint EridaniStarbase = new("Eridani Starbase",
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Orion
        public static readonly Blueprint OrionInterceptor = new("Orion Interceptor",
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, Part.GaussShield)
        { BaseInitiative = 3, BaseEnergy = 1 };
        public static readonly Blueprint OrionCruiser = new("Orion Cruiser",
            Part.ElectronComputer, Part.IonCannon, Part.GaussShield,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 2, BaseEnergy = 2 };
        public static readonly Blueprint OrionDreadnaught = new("Orion Dreadnaught",
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, Part.GaussShield,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1, BaseEnergy = 3 };
        public static readonly Blueprint OrionStarbase = new("Orion Starbase",
            Part.ElectronComputer, Part.GaussShield,
            Part.Hull, Part.IonCannon, Part.Hull)
        { BaseInitiative = 5, BaseEnergy = 3, IsBase = true };

        //Planta
        public static readonly Blueprint PlantaInterceptor = new("Planta Interceptor",
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive)
        { BaseEnergy = 2, BaseComputer = 1 };
        public static readonly Blueprint PlantaCruiser = new("Planta Cruiser",
            Part.NuclearSource, Part.IonCannon, null,
            Part.Hull, Part.NuclearDrive)
        { BaseEnergy = 2, BaseComputer = 1 };
        public static readonly Blueprint PlantaDreadnaught = new("Planta Dreadnaught",
            Part.NuclearSource, Part.IonCannon, Part.IonCannon, null,
            Part.Hull, Part.Hull, Part.NuclearDrive)
        { BaseEnergy = 2, BaseComputer = 1 };
        public static readonly Blueprint PlantaStarbase = new("Planta Starbase",
            Part.ElectronComputer, Part.Hull,
            Part.IonCannon, Part.Hull)
        { BaseInitiative = 2, BaseEnergy = 5, BaseComputer = 1, IsBase = true };

        //Rho Indi
        public static readonly Blueprint RhoIndiInterceptor = new("Rho Indi Interceptor",
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 3, BaseShield = 1 };
        public static readonly Blueprint RhoIndiCruiser = new("Rho Indi Cruiser",
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 2, BaseShield = 1 };
        public static readonly Blueprint RhoIndiStarbase = new("Rho Indi Starbase",
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, BaseShield = 1, IsBase = true };

        //The Exiles
        public static readonly Blueprint ExilesInterceptor = new("Exiles Interceptor",
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, Part.ElectronComputer)
        { BaseInitiative = 2 };
        public static readonly Blueprint ExilesCruiser = new("Exiles Cruiser",
            Part.ElectronComputer, Part.IonCannon, Part.ElectronComputer,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint ExilesDreadnaught = new("Exiles Dreadnaught",
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, Part.ElectronComputer,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint ExilesOrbital = new("Exiles Orbital",
            Part.Hull, Part.IonTurret, Part.ElectronComputer)
        { BaseHull = 2, BaseEnergy = 4, IsBase = true };
    }
}