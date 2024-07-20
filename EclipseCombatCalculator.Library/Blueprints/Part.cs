using EclipseCombatCalculator.Library.Dices;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EclipseCombatCalculator.Library.Blueprints
{
    public sealed class Part
    {
        public string Name { get; }
        [JsonIgnore]
        public IEnumerable<Dice> Cannons { get; private init; } = Array.Empty<Dice>();
        [JsonIgnore]
        public IEnumerable<Dice> Missiles { get; private init; } = Array.Empty<Dice>();
        [JsonIgnore]
        public int Computers { get; private init; } = 0;
        [JsonIgnore]
        public int Shields { get; private init; } = 0;
        [JsonIgnore]
        public int Hulls { get; private init; } = 0;
        [JsonIgnore]
        public int Initiative { get; private init; } = 0;
        [JsonIgnore]
        public int Energy { get; private init; } = 0;
        [JsonIgnore]
        public int Movement { get; private init; } = 0;
        [JsonIgnore]
        public PartSource Source { get; private init; } = PartSource.Technology;
        [JsonIgnore]
        public bool OutsideBlueprint { get; private init; } = false;

        private Part(string name)
        {
            this.Name = name;
            allParts.Add(this);
        }

        private static readonly List<Part> allParts = new();

        public static IEnumerable<Part> AllPArts => allParts;

        // Weapons
        public static readonly Part IonCannon = new("Ion Cannon") { Cannons = new[] { CommonDices.YellowDice }, Energy = -1, Source = PartSource.Initial };
        public static readonly Part PlasmaCannon = new("Plasma Cannon") { Cannons = new[] { CommonDices.OrangeDice }, Energy = -2 };
        public static readonly Part SolitonCannon = new("Soliton Cannon") { Cannons = new[] { CommonDices.BlueDice }, Energy = -3 };
        public static readonly Part AntimatterCannon = new("Antimatter Cannon") { Cannons = new[] { CommonDices.RedDice }, Energy = -4 };
        public static readonly Part RiftCannon = new("Rift Cannon") { Cannons = new[] { CommonDices.PurpleDice }, Energy = -2 };
        public static readonly Part FluxMissile = new("Flux Missile") { Missiles = new[] { CommonDices.YellowDice, CommonDices.YellowDice } };
        public static readonly Part PlasmaMissile = new("Plasma Missile") { Missiles = new[] { CommonDices.OrangeDice, CommonDices.OrangeDice }, Energy = -1 };

        // Computers
        public static readonly Part ElectronComputer = new("Electron Computer") { Computers = 1, Source = PartSource.Initial };
        public static readonly Part PositronComputer = new("Positron Computer") { Computers = 2, Energy = -1 };
        public static readonly Part GluonComputer = new("Gluon Computer") { Computers = 3, Energy = -2 };

        // Shields
        public static readonly Part GaussShield = new("Gauss Shield") { Shields = 1 };
        public static readonly Part PhaseShield = new("Phase Shield") { Shields = 2, Energy = -1 };
        public static readonly Part AbsorptionShield = new("Absorption Shield") { Shields = 1, Energy = 4 };

        // Hulls
        public static readonly Part Hull = new("Hull") { Hulls = 1, Source = PartSource.Initial };
        public static readonly Part ImprovedHull = new("Improved Hull") { Hulls = 2 };
        public static readonly Part ConifoldField = new("Conifold Field") { Hulls = 3, Energy = -2 };

        // Drives
        public static readonly Part NuclearDrive = new("Nuclear Drive") { Movement = 1, Initiative = 1, Energy = -1, Source = PartSource.Initial };
        public static readonly Part FusionDrive = new("Fusion Drive") { Movement = 2, Initiative = 2, Energy = -2 };
        public static readonly Part TachyonDrive = new("Tachyon Drive") { Movement = 3, Initiative = 3, Energy = -3 };
        public static readonly Part TransitionDrive = new("Transition Drive") { Movement = 3 };

        // Energy sources
        public static readonly Part NuclearSource = new("Nuclear Source") { Energy = 3, Source = PartSource.Initial };
        public static readonly Part FusionSource = new("Fusion Source") { Energy = 6 };
        public static readonly Part TachyonSource = new("Tachyon Source") { Energy = 9 };
        public static readonly Part ZeroPointSource = new("Zero-Point Source") { Energy = 12 };

        // Ancient ship parts
        public static readonly Part IonDisruptor = new("Ion Disruptor") { Cannons = new[] { CommonDices.YellowDice }, Initiative = 4, Source = PartSource.Discovery };
        public static readonly Part IonTurret = new("Ion Turret") { Cannons = new[] { CommonDices.YellowDice, CommonDices.YellowDice }, Source = PartSource.Discovery };
        public static readonly Part PlasmaTurret = new("Plasma Turret") { Cannons = new[] { CommonDices.OrangeDice, CommonDices.OrangeDice }, Energy = -3, Source = PartSource.Discovery };
        public static readonly Part SolitonCharger = new("Soliton Charger") { Cannons = new[] { CommonDices.BlueDice }, Energy = -1, Source = PartSource.Discovery };
        public static readonly Part RiftConductor = new("Rift Conductor") { Cannons = new[] { CommonDices.PurpleDice }, Hulls = 1, Energy = -1, Source = PartSource.Discovery };
        public static readonly Part IonMissile = new("Ion Missile") { Missiles = new[] { CommonDices.YellowDice, CommonDices.YellowDice, CommonDices.YellowDice }, Source = PartSource.Discovery };
        public static readonly Part SolitonMissile = new("Soliton Missile") { Missiles = new[] { CommonDices.BlueDice }, Initiative = 1, Source = PartSource.Discovery };
        public static readonly Part AntimatterMissile = new("Antimatter Missile") { Missiles = new[] { CommonDices.RedDice }, Source = PartSource.Discovery };
        public static readonly Part AxionComputer = new("Axion Computer") { Computers = 2, Initiative = 1, Source = PartSource.Discovery };
        public static readonly Part FluxShield = new("Flux Shield") { Shields = 3, Initiative = 1, Energy = -2, Source = PartSource.Discovery };
        public static readonly Part InversionShield = new("Inversion Shield") { Shields = 2, Energy = 2, Source = PartSource.Discovery };
        public static readonly Part ShardHull = new("Shard Hull") { Hulls = 3, Source = PartSource.Discovery };
        public static readonly Part ConformalDrive = new("Conformal Drive") { Movement = 4, Initiative = 2, Energy = -2, Source = PartSource.Discovery };
        public static readonly Part NonLinearDrive = new("Nonlinear Drive") { Movement = 2, Energy = 2, Source = PartSource.Discovery };
        public static readonly Part HypergridSource = new("Hypergrid Source") { Energy = 11, Source = PartSource.Discovery };

        // Esoteric parts
        public static readonly Part JumpDrive = new("Jump Drive") { Energy = -2, Source = PartSource.Discovery };
        public static readonly Part MorphShield = new("Morph Shield") { Initiative = 1, Shields = 1, Source = PartSource.Discovery }; // TODO: Implement
        public static readonly Part MuonSource = new("Muon Source") { Initiative = 1, Energy = 2, Source = PartSource.Discovery, OutsideBlueprint = true };
    }
}
