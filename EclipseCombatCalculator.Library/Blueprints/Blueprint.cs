using EclipseCombatCalculator.Library.Dices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EclipseCombatCalculator.Library.Blueprints
{
    public sealed class Blueprint : IShipStats
    {
        public ShipType ShipType { get; }
        public Species Species { get; }
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

        public string Name
        {
            get => name;
            set
            {
                if (readOnlyBlueprint)
                {
                    throw new InvalidOperationException("Trying to edit read only Blueprint");
                }
                name = value;
            }
        }

        public int Size => slots.Length;

        public IEnumerable<Part> Parts => slots;

        public bool CanEdit => !readOnlyBlueprint;

        private string name;
        readonly Part[] slots;
        readonly bool readOnlyBlueprint;


        private Blueprint(string name, ShipType type, Species species, params Part[] parts)
        {
            this.name = name;
            this.ShipType = type;
            this.Species = species;
            this.slots = parts;
            readOnlyBlueprint = true;
            blueprints.Add(this);
        }
        private Blueprint(Blueprint other, bool readOnlyBlueprint)
        {
            this.name = other.Name;
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
        }

        public Blueprint CreateEditableClone()
        {
            return new Blueprint(this, false);
        }

        private Blueprint Clone(string name)
        {
            return new Blueprint(this, true)
            {
                name = name
            };
        }

        public int TotalEnergy => BaseEnergy + slots.Sum(part => part?.Energy) ?? 0;

        int IShipStats.Initiative => BaseInitiative + slots.Sum(part => part?.Initiative) ?? 0;

        int IShipStats.Computers => BaseComputer + slots.Sum(part => part?.Computers) ?? 0;

        int IShipStats.Shields => BaseShield + slots.Sum(part => part?.Shields) ?? 0;

        int IShipStats.Hulls => BaseHull + slots.Sum(part => part?.Hulls) ?? 0;

        IEnumerable<Dice> IShipStats.Cannons => slots.SelectMany(part => part?.Cannons ?? Array.Empty<Dice>());

        IEnumerable<Dice> IShipStats.Missiles => slots.SelectMany(part => part?.Missiles ?? Array.Empty<Dice>());


        readonly static List<Blueprint> blueprints = new();

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
        public static readonly Blueprint MechamenaInterceptor = new("Mechamena Interceptor", ShipType.Interceptor, Species.Mechamena,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint MechamenaCruiser = new("Mechamena Cruiser", ShipType.Cruiser, Species.Mechamena,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint MechamenaDreadnaught = new("Mechamena Dreadnaught", ShipType.Dreadnaught, Species.Mechamena,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint MechamenaStarbase = new("Mechamena Starbase", ShipType.Starbase, Species.Mechamena,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Hydran
        public static readonly Blueprint HydranInterceptor = new("Hydran Interceptor", ShipType.Interceptor, Species.Hydran,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint HydranCruiser = new("Hydran Cruiser", ShipType.Cruiser, Species.Hydran,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint HydranDreadnaught = new("Hydran Dreadnaught", ShipType.Dreadnaught, Species.Hydran,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint HydranStarbase = new("Hydran Starbase", ShipType.Starbase, Species.Hydran,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Draco
        public static readonly Blueprint DracoInterceptor = new("Draco Interceptor", ShipType.Interceptor, Species.Draco,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint DracoCruiser = new("Draco Cruiser", ShipType.Cruiser, Species.Draco,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint DracoDreadnaught = new("Draco Dreadnaught", ShipType.Dreadnaught, Species.Draco,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint DracoStarbase = new("Draco Starbase", ShipType.Starbase, Species.Draco,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Magellan
        public static readonly Blueprint MagellanInterceptor = new("Magellan Interceptor", ShipType.Interceptor, Species.Magellan,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint MagellanCruiser = new("Magellan Cruiser", ShipType.Cruiser, Species.Magellan,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint MagellanDreadnaught = new("Magellan Dreadnaught", ShipType.Dreadnaught, Species.Magellan,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint MagellanStarbase = new("Magellan Starbase", ShipType.Starbase, Species.Magellan,
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Lyra
        public static readonly Blueprint LyraInterceptor = new ("Lyra Interceptor", ShipType.Interceptor, Species.Lyra,
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint LyraCruiser = new ("Lyra Cruiser", ShipType.Cruiser, Species.Lyra,
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint LyraDreadnaught = new ("Lyra Dreadnaught", ShipType.Dreadnaught, Species.Lyra,
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint LyraStarbase = new("Lyra Starbase", ShipType.Starbase, Species.Lyra,
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
            Part.ElectronComputer, Part.Hull, Part.IonTurret)
        { BaseHull = 2, BaseEnergy = 4, IsBase = true };

        public static IEnumerable<Blueprint> Blueprints => blueprints;

        public static IEnumerable<Blueprint> GetBlueprints(Species species)
        {
            return Blueprints.Where(blueprint => blueprint.Species == species);
        }
    }
}
