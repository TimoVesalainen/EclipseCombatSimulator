using EclipseCombatCalculator.Library.Dices;
using System;
using System.Collections.Generic;

namespace EclipseCombatCalculator.Library.Blueprints
{
    public sealed class Part
    {
        public string Name { get; }
        public IEnumerable<Dice> Cannons { get; private init; } = Array.Empty<Dice>();
        public IEnumerable<Dice> Missiles { get; private init; } = Array.Empty<Dice>();
        public int Computers { get; private init; } = 0;
        public int Shields { get; private init; } = 0;
        public int Hulls { get; private init; } = 0;
        public int Initiative { get; private init; } = 0;
        public int Energy { get; private init; } = 0;
        public int Movement { get; private init; } = 0;
        public PartSource Source { get; private init; } = PartSource.Technology;

        private Part(string name)
        {
            this.Name = name;
            allParts.Add(this);
        }

        private static readonly List<Part> allParts = new();

        public static IEnumerable<Part> AllPArts => allParts;

        // Weapons
        public static readonly Part IonCannon = new Part("Ion Cannon") { Cannons = new[] { CommonDices.YellowDice }, Energy = -1, Source = PartSource.Initial };
        public static readonly Part PlasmaCannon = new Part("Plasma Cannon") { Cannons = new[] { CommonDices.OrangeDice }, Energy = -2 };
        public static readonly Part SolitonCannon = new Part("Soliton Cannon") { Cannons = new[] { CommonDices.BlueDice }, Energy = -3 };
        public static readonly Part AntimatterCannon = new Part("Antimatter Cannon") { Cannons = new[] { CommonDices.RedDice }, Energy = -4 };
        public static readonly Part RiftCannon = new Part("Rift Cannon") { Cannons = new[] { CommonDices.PurpleDice }, Energy = -2 };
        public static readonly Part FluxMissile = new Part("Flux Missile") { Missiles = new[] { CommonDices.YellowDice, CommonDices.YellowDice } };
        public static readonly Part PlasmaMissile = new Part("Plasma Missile") { Missiles = new[] { CommonDices.OrangeDice, CommonDices.OrangeDice }, Energy = -1 };

        // Computers
        public static readonly Part ElectronComputer = new Part("Electron Computer") { Computers = 1, Source = PartSource.Initial };
        public static readonly Part PositronComputer = new Part("Positron Computer") { Computers = 2, Energy = -1 };
        public static readonly Part GluonComputer = new Part("Gluon Computer") { Computers = 3, Energy = -2 };

        // Shields
        public static readonly Part GaussShield = new Part("Gauss Shield") { Shields = 1 };
        public static readonly Part PhaseShield = new Part("Phase Shield") { Shields = 2, Energy = -1 };
        public static readonly Part AbsorptionShield = new Part("Absorption Shield") { Shields = 1, Energy = 4 };

        // Hulls
        public static readonly Part Hull = new Part("Hull") { Hulls = 1, Source = PartSource.Initial };
        public static readonly Part ImprovedHull = new Part("Improved Hull") { Hulls = 2 };
        public static readonly Part ConifoldField = new Part("Conifold Field") { Hulls = 3, Energy = -2 };

        // Drives
        public static readonly Part NuclearDrive = new Part("Nuclear Drive") { Movement = 1, Initiative = 1, Energy = -1, Source = PartSource.Initial };
        public static readonly Part FusionDrive = new Part("Fusion Drive") { Movement = 2, Initiative = 2, Energy = -2 };
        public static readonly Part TachyonDrive = new Part("Tachyon Drive") { Movement = 3, Initiative = 3, Energy = -3 };
        public static readonly Part TransitionDrive = new Part("Transition Drive") { Movement = 3 };

        // Energy sources
        public static readonly Part NuclearSource = new Part("Nuclear Source") { Energy = 3, Source = PartSource.Initial };
        public static readonly Part FusionSource = new Part("Fusion Source") { Energy = 6 };
        public static readonly Part TachyonSource = new Part("Tachyon Source") { Energy = 9 };
        public static readonly Part ZeroPointSource = new Part("Zero-Point Source") { Energy = 12 };

        // Ancient ship parts
        public static readonly Part IonDisruptor = new Part("Ion Disruptor") { Cannons = new[] { CommonDices.YellowDice }, Initiative = 4, Source = PartSource.Discovery };
        public static readonly Part IonTurret = new Part("Ion Turret") { Cannons = new[] { CommonDices.YellowDice, CommonDices.YellowDice }, Source = PartSource.Discovery };
        public static readonly Part PlasmaTurret = new Part("Plasma Turret") { Cannons = new[] { CommonDices.OrangeDice, CommonDices.OrangeDice }, Energy = -3, Source = PartSource.Discovery };
        public static readonly Part SolitonCharger = new Part("Soliton Charger") { Cannons = new[] { CommonDices.BlueDice }, Energy = -1, Source = PartSource.Discovery };
        public static readonly Part RiftConductor = new Part("Rift Conductor") { Cannons = new[] { CommonDices.PurpleDice }, Hulls = 1, Energy = -1, Source = PartSource.Discovery };
        public static readonly Part IonMissile = new Part("Ion Missile") { Missiles = new[] { CommonDices.YellowDice, CommonDices.YellowDice, CommonDices.YellowDice }, Source = PartSource.Discovery };
        public static readonly Part SolitonMissile = new Part("Soliton Missile") { Missiles = new[] { CommonDices.BlueDice }, Initiative = 1, Source = PartSource.Discovery };
        public static readonly Part AntimatterMissile = new Part("Antimatter Missile") { Missiles = new[] { CommonDices.RedDice }, Source = PartSource.Discovery };
        public static readonly Part AxionComputer = new Part("Axion Computer") { Computers = 2, Initiative = 1, Source = PartSource.Discovery };
        public static readonly Part FluxShield = new Part("Flux Shield") { Shields = 3, Initiative = 1, Energy = -2, Source = PartSource.Discovery };
        public static readonly Part InversionShield = new Part("Inversion Shield") { Shields = 2, Energy = 2, Source = PartSource.Discovery };
        public static readonly Part ShardHull = new Part("Shard Hull") { Hulls = 3, Source = PartSource.Discovery };
        public static readonly Part ConformalDrive = new Part("Conformal Drive") { Movement = 4, Initiative = 2, Energy = -2, Source = PartSource.Discovery };
        public static readonly Part NonLinearDrive = new Part("Nonlinear Drive") { Movement = 2, Energy = 2, Source = PartSource.Discovery };
        public static readonly Part HypergridSource = new Part("Hypergrid Source") { Energy = 11, Source = PartSource.Discovery };

        // Esoteric parts
        public static readonly Part JumpDrive = new Part("Jump Drive") { Energy = -2, Source = PartSource.Discovery };
        public static readonly Part MorphShield = new Part("Morph Shield") { Initiative = 1, Shields = 1, Source = PartSource.Discovery }; // TODO
        public static readonly Part MuonSource = new Part("Muon Source") { Initiative = 1, Energy = 2, Source = PartSource.Discovery };
    }
}
