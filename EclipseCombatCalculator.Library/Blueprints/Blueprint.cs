﻿using EclipseCombatCalculator.Library.Dices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EclipseCombatCalculator.Library.Blueprints
{
    public sealed class Blueprint : IShipStats
    {
        public ShipType ShipType { get; }
        public Species Species { get; }
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
            set
            {
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

        private Blueprint(string name, ShipType type, Species species, params Part[] parts)
        {
            Name = name;
            this.ShipType = type;
            this.Species = species;
            this.slots = parts;
            readOnlyBlueprint = true;
            blueprints.Add(this);
        }
        private Blueprint(Blueprint other, bool readOnlyBlueprint)
        {
            Name = other.Name;
            this.Species = other.Species;
            this.ShipType = other.ShipType;
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


        readonly static List<Blueprint> blueprints = new();

        public static IEnumerable<Blueprint> Blueprints => blueprints;

        //Terran
        public static readonly Blueprint TerranInterceptor = new("Terran Interceptor", ShipType.Interceptor, Species.Terran,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint TerranCruiser = new("Terran Cruiser", ShipType.Cruiser, Species.Terran,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint TerranDreadnaught = new("Terran Dreadnaught", ShipType.Dreadnaught, Species.Terran,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint TerranStarbase = new("Terran Starbase", ShipType.Starbase, Species.Terran,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Mechanema
        public static readonly Blueprint MechamenaInterceptor = new("Terran Interceptor", ShipType.Interceptor, Species.Mechamena,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint MechamenaCruiser = new("Terran Cruiser", ShipType.Cruiser, Species.Mechamena,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint MechamenaDreadnaught = new("Terran Dreadnaught", ShipType.Dreadnaught, Species.Mechamena,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint MechamenaStarbase = new("Terran Starbase", ShipType.Starbase, Species.Mechamena,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Hydran
        public static readonly Blueprint HydranInterceptor = new("Terran Interceptor", ShipType.Interceptor, Species.Hydran,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint HydranCruiser = new("Terran Cruiser", ShipType.Cruiser, Species.Hydran,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint HydranDreadnaught = new("Terran Dreadnaught", ShipType.Dreadnaught, Species.Hydran,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint HydranStarbase = new("Terran Starbase", ShipType.Starbase, Species.Hydran,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Draco
        public static readonly Blueprint DracoInterceptor = new("Terran Interceptor", ShipType.Interceptor, Species.Draco,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint DracoCruiser = new("Terran Cruiser", ShipType.Cruiser, Species.Draco,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint DracoDreadnaught = new("Terran Dreadnaught", ShipType.Dreadnaught, Species.Draco,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint DracoStarbase = new("Terran Starbase", ShipType.Starbase, Species.Draco,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Magellan
        public static readonly Blueprint MagellanInterceptor = new("Terran Interceptor", ShipType.Interceptor, Species.Magellan,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint MagellanCruiser = new("Terran Cruiser", ShipType.Cruiser, Species.Magellan,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint MagellanDreadnaught = new("Terran Dreadnaught", ShipType.Dreadnaught, Species.Magellan,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint MagellanStarbase = new("Terran Starbase", ShipType.Starbase, Species.Magellan,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Lyra
        public static readonly Blueprint LyraInterceptor = new ("Terran Interceptor", ShipType.Interceptor, Species.Lyra,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint LyraCruiser = new ("Terran Cruiser", ShipType.Cruiser, Species.Lyra,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint LyraDreadnaught = new ("Terran Dreadnaught", ShipType.Dreadnaught, Species.Lyra,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint LyraStarbase = new("Terran Starbase", ShipType.Starbase, Species.Lyra,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Eridani
        public static readonly Blueprint EridaniInterceptor = new("Eridani Interceptor", ShipType.Interceptor, Species.Eridani,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2, BaseEnergy = 1 };
        public static readonly Blueprint EridaniCruiser = new("Eridani Cruiser", ShipType.Cruiser, Species.Eridani,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1, BaseEnergy = 1 };
        public static readonly Blueprint EridaniDreadnaught = new("Eridani Dreadnaught", ShipType.Dreadnaught, Species.Eridani,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive)
        { BaseEnergy = 1 };
        public static readonly Blueprint EridaniStarbase = new("Eridani Starbase", ShipType.Starbase, Species.Eridani,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Orion
        public static readonly Blueprint OrionInterceptor = new("Orion Interceptor", ShipType.Interceptor, Species.Orion,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, Part.GaussShield)
        { BaseInitiative = 3, BaseEnergy = 1 };
        public static readonly Blueprint OrionCruiser = new("Orion Cruiser", ShipType.Cruiser, Species.Orion,
            Part.ElectronComputer, Part.IonCannon, Part.GaussShield,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 2, BaseEnergy = 2 };
        public static readonly Blueprint OrionDreadnaught = new("Orion Dreadnaught", ShipType.Dreadnaught, Species.Orion,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, Part.GaussShield,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1, BaseEnergy = 3 };
        public static readonly Blueprint OrionStarbase = new("Orion Starbase", ShipType.Starbase, Species.Orion,
            Part.ElectronComputer, Part.GaussShield,
            Part.Hull, Part.IonCannon, Part.Hull)
        { BaseInitiative = 5, BaseEnergy = 3, IsBase = true };

        //Planta
        public static readonly Blueprint PlantaInterceptor = new("Planta Interceptor", ShipType.Interceptor, Species.Planta,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive)
        { BaseEnergy = 2, BaseComputer = 1 };
        public static readonly Blueprint PlantaCruiser = new("Planta Cruiser", ShipType.Cruiser, Species.Planta,
            Part.NuclearSource, Part.IonCannon, null,
            Part.Hull, Part.NuclearDrive)
        { BaseEnergy = 2, BaseComputer = 1 };
        public static readonly Blueprint PlantaDreadnaught = new("Planta Dreadnaught", ShipType.Dreadnaught, Species.Planta,
            Part.NuclearSource, Part.IonCannon, Part.IonCannon, null,
            Part.Hull, Part.Hull, Part.NuclearDrive)
        { BaseEnergy = 2, BaseComputer = 1 };
        public static readonly Blueprint PlantaStarbase = new("Planta Starbase", ShipType.Starbase, Species.Planta,
            Part.ElectronComputer, Part.Hull,
            Part.IonCannon, Part.Hull)
        { BaseInitiative = 2, BaseEnergy = 5, BaseComputer = 1, IsBase = true };

        //Rho Indi
        public static readonly Blueprint RhoIndiInterceptor = new("Rho Indi Interceptor", ShipType.Interceptor, Species.RhoIndi,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 3, BaseShield = 1 };
        public static readonly Blueprint RhoIndiCruiser = new("Rho Indi Cruiser", ShipType.Cruiser, Species.RhoIndi,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 2, BaseShield = 1 };
        public static readonly Blueprint RhoIndiStarbase = new("Rho Indi Starbase", ShipType.Starbase, Species.RhoIndi,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, BaseShield = 1, IsBase = true };

        //The Exiles
        public static readonly Blueprint ExilesInterceptor = new("Exiles Interceptor", ShipType.Interceptor, Species.Exiles,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, Part.ElectronComputer)
        { BaseInitiative = 2 };
        public static readonly Blueprint ExilesCruiser = new("Exiles Cruiser", ShipType.Cruiser, Species.Exiles,
            Part.ElectronComputer, Part.IonCannon, Part.ElectronComputer,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint ExilesDreadnaught = new("Exiles Dreadnaught", ShipType.Dreadnaught, Species.Exiles,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, Part.ElectronComputer,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint ExilesOrbital = new("Exiles Orbital", ShipType.Starbase, Species.Exiles,
            Part.Hull, Part.IonTurret, Part.ElectronComputer)
        { BaseHull = 2, BaseEnergy = 4, IsBase = true };
    }
}
