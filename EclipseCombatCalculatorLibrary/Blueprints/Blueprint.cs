using EclipseCombatCalculatorLibrary.Dices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EclipseCombatCalculatorLibrary.Blueprints
{
    public sealed class Blueprint : IShipStats
    {
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

        private Blueprint(params Part[] parts)
        {
            this.slots = parts;
            readOnlyBlueprint = true;
        }
        private Blueprint(Blueprint other, bool readOnlyBlueprint)
        {
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

        public int TotalEnergy => BaseEnergy + Parts.Sum(part => part.Energy);

        int IShipStats.Initiative => BaseInitiative + Parts.Sum(part => part.Initiative);

        int IShipStats.Computers => BaseComputer + Parts.Sum(part => part.Computers);

        int IShipStats.Shields => BaseShield + Parts.Sum(part => part.Shields);

        int IShipStats.Hulls => BaseHull + Parts.Sum(part => part.Hulls);

        IEnumerable<Dice> IShipStats.Cannons => Parts.SelectMany(part => part.Cannons);

        IEnumerable<Dice> IShipStats.Missiles => Parts.SelectMany(part => part.Missiles);

        //Default aka Terran, Mechanema, Hydran, Draco, Magellan, Lyra
        public static readonly Blueprint TerranInterceptor = new(
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2 };
        public static readonly Blueprint TerranCruiser = new(
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint TerranDreadnaught = new(
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint TerranStarbase = new(
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Eridani
        public static readonly Blueprint EridaniInterceptor = new(
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 2, BaseEnergy = 1 };
        public static readonly Blueprint EridaniCruiser = new(
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1, BaseEnergy = 1 };
        public static readonly Blueprint EridaniDreadnaught = new(
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive)
        { BaseEnergy = 1 };
        public static readonly Blueprint EridaniStarbase = new(
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, IsBase = true };

        //Orion
        public static readonly Blueprint OrionInterceptor = new(
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, Part.GaussShield)
        { BaseInitiative = 3, BaseEnergy = 1 };
        public static readonly Blueprint OrionCruiser = new(
            Part.ElectronComputer, Part.IonCannon, Part.GaussShield,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 2, BaseEnergy = 2 };
        public static readonly Blueprint OrionDreadnaught = new(
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, Part.GaussShield,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1, BaseEnergy = 3 };
        public static readonly Blueprint OrionStarbase = new(
            Part.ElectronComputer, Part.GaussShield,
            Part.Hull, Part.IonCannon, Part.Hull)
        { BaseInitiative = 5, BaseEnergy = 3, IsBase = true };

        //Planta
        public static readonly Blueprint PlantaInterceptor = new(
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive)
        { BaseEnergy = 2, BaseComputer = 1 };
        public static readonly Blueprint PlantaCruiser = new(
            Part.NuclearSource, Part.IonCannon, null,
            Part.Hull, Part.NuclearDrive)
        { BaseEnergy = 2, BaseComputer = 1 };
        public static readonly Blueprint PlantaDreadnaught = new(
            Part.NuclearSource, Part.IonCannon, Part.IonCannon, null,
            Part.Hull, Part.Hull, Part.NuclearDrive)
        { BaseEnergy = 2, BaseComputer = 1 };
        public static readonly Blueprint PlantaStarbase = new(
            Part.ElectronComputer, Part.Hull,
            Part.IonCannon, Part.Hull)
        { BaseInitiative = 2, BaseEnergy = 5, BaseComputer = 1, IsBase = true };

        //Rho Indi
        public static readonly Blueprint RhoIndiInterceptor = new(
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, null)
        { BaseInitiative = 3, BaseShield = 1 };
        public static readonly Blueprint RhoIndiCruiser = new(
            Part.ElectronComputer, Part.IonCannon, null,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 2, BaseShield = 1 };
        public static readonly Blueprint RhoIndiStarbase = new(
            Part.ElectronComputer, Part.IonCannon,
            Part.Hull, null, Part.Hull)
        { BaseInitiative = 4, BaseEnergy = 3, BaseShield = 1, IsBase = true };

        //The Exiles
        public static readonly Blueprint ExilesInterceptor = new(
            Part.IonCannon,
            Part.NuclearSource, Part.NuclearDrive, Part.ElectronComputer)
        { BaseInitiative = 2 };
        public static readonly Blueprint ExilesCruiser = new(
            Part.ElectronComputer, Part.IonCannon, Part.ElectronComputer,
            Part.NuclearSource, Part.Hull, Part.NuclearDrive)
        { BaseInitiative = 1 };
        public static readonly Blueprint ExilesDreadnaught = new(
            Part.ElectronComputer, Part.IonCannon, Part.IonCannon, Part.ElectronComputer,
            Part.NuclearSource, Part.Hull, Part.Hull, Part.NuclearDrive);
        public static readonly Blueprint ExilesOrbital = new(
            Part.Hull, Part.IonTurret, Part.ElectronComputer)
        { BaseHull = 2, BaseEnergy = 4, IsBase = true };
    }
}
