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
        public IEnumerable<Dice> Cannons { get; }
        [JsonIgnore]
        public IEnumerable<Dice> Missiles { get; }
        [JsonIgnore]
        public int Computers { get; } = 0;
        [JsonIgnore]
        public int Shields { get; } = 0;
        [JsonIgnore]
        public int Hulls { get; } = 0;
        [JsonIgnore]
        public int Initiative { get; } = 0;
        [JsonIgnore]
        public int Energy { get; } = 0;
        [JsonIgnore]
        public int Movement { get; } = 0;
        [JsonIgnore]
        public PartSource Source { get; }
        [JsonIgnore]
        public bool OutsideBlueprint { get; } = false;

        [JsonConstructor]
        public Part(string name)
        {
            this.Name = name;
        }

        private Part(string name,
            IEnumerable<Dice> cannons = null,
            IEnumerable<Dice> missiles = null,
            int computers = 0,
            int shields = 0,
            int hulls = 0,
            int initiative = 0,
            int energy = 0,
            int movement = 0,
            PartSource source = PartSource.Technology,
            bool outsideBlueprint = false
            )
        {
            this.Name = name;
            Cannons = cannons ?? Array.Empty<Dice>();
            Missiles = missiles ?? Array.Empty<Dice>();
            Computers = computers;
            Shields = shields;
            Hulls = hulls;
            Initiative = initiative;
            Energy = energy;
            Movement = movement;
            Source = source;
            OutsideBlueprint = outsideBlueprint;

            allParts.Add(this);
        }


        private static readonly List<Part> allParts = new();

        public static IEnumerable<Part> AllPArts => allParts;

        public static Part FindByName(string name)
        {
            foreach (Part part in allParts)
            {
                if (part.Name == name)
                {
                    return part;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        // Weapons
        public static readonly Part IonCannon = new("Ion Cannon", cannons: new[] { CommonDices.YellowDice }, energy: -1, source: PartSource.Initial );
        public static readonly Part PlasmaCannon = new("Plasma Cannon", cannons: new[] { CommonDices.OrangeDice }, energy: -2 );
        public static readonly Part SolitonCannon = new("Soliton Cannon", cannons: new[] { CommonDices.BlueDice }, energy: -3);
        public static readonly Part AntimatterCannon = new("Antimatter Cannon", cannons: new[] { CommonDices.RedDice }, energy: -4);
        public static readonly Part RiftCannon = new("Rift Cannon", cannons: new[] { CommonDices.PurpleDice }, energy: -2);
        public static readonly Part FluxMissile = new("Flux Missile", missiles: new[] { CommonDices.YellowDice, CommonDices.YellowDice });
        public static readonly Part PlasmaMissile = new("Plasma Missile", missiles: new[] { CommonDices.OrangeDice, CommonDices.OrangeDice }, energy: -1);

        // Computers
        public static readonly Part ElectronComputer = new("Electron Computer", computers: 1, source: PartSource.Initial);
        public static readonly Part PositronComputer = new("Positron Computer", computers: 2, energy: -1);
        public static readonly Part GluonComputer = new("Gluon Computer", computers: 3, energy: -2);

        // Shields
        public static readonly Part GaussShield = new("Gauss Shield", shields: 1);
        public static readonly Part PhaseShield = new("Phase Shield", shields: 2, energy: -1);
        public static readonly Part AbsorptionShield = new("Absorption Shield", shields: 1, energy: 4);

        // Hulls
        public static readonly Part Hull = new("Hull", hulls: 1, source: PartSource.Initial);
        public static readonly Part ImprovedHull = new("Improved Hull", hulls: 2);
        public static readonly Part ConifoldField = new("Conifold Field", hulls: 3, energy: -2);

        // Drives
        public static readonly Part NuclearDrive = new("Nuclear Drive", movement: 1, initiative: 1, energy: -1, source: PartSource.Initial);
        public static readonly Part FusionDrive = new("Fusion Drive", movement: 2, initiative: 2, energy: -2);
        public static readonly Part TachyonDrive = new("Tachyon Drive", movement: 3, initiative: 3, energy: -3);
        public static readonly Part TransitionDrive = new("Transition Drive", movement: 3);

        // Energy sources
        public static readonly Part NuclearSource = new("Nuclear Source", energy: 3, source: PartSource.Initial);
        public static readonly Part FusionSource = new("Fusion Source", energy: 6);
        public static readonly Part TachyonSource = new("Tachyon Source", energy: 9);
        public static readonly Part ZeroPointSource = new("Zero-Point Source", energy: 12);

        // Ancient ship parts
        public static readonly Part IonDisruptor = new("Ion Disruptor", cannons: new[] { CommonDices.YellowDice }, initiative: 4, source: PartSource.Discovery);
        public static readonly Part IonTurret = new("Ion Turret", cannons: new[] { CommonDices.YellowDice, CommonDices.YellowDice }, source: PartSource.Discovery);
        public static readonly Part PlasmaTurret = new("Plasma Turret", cannons: new[] { CommonDices.OrangeDice, CommonDices.OrangeDice }, energy: -3, source: PartSource.Discovery);
        public static readonly Part SolitonCharger = new("Soliton Charger", cannons: new[] { CommonDices.BlueDice }, energy: -1, source: PartSource.Discovery);
        public static readonly Part RiftConductor = new("Rift Conductor", cannons: new[] { CommonDices.PurpleDice }, hulls: 1, energy: -1, source: PartSource.Discovery);
        public static readonly Part IonMissile = new("Ion Missile", missiles: new[] { CommonDices.YellowDice, CommonDices.YellowDice, CommonDices.YellowDice }, source: PartSource.Discovery );
        public static readonly Part SolitonMissile = new("Soliton Missile", missiles: new[] { CommonDices.BlueDice }, initiative: 1, source: PartSource.Discovery );
        public static readonly Part AntimatterMissile = new("Antimatter Missile", missiles: new[] { CommonDices.RedDice }, source: PartSource.Discovery );
        public static readonly Part AxionComputer = new("Axion Computer", computers: 2, initiative: 1, source: PartSource.Discovery );
        public static readonly Part FluxShield = new("Flux Shield", shields: 3, initiative: 1, energy: -2, source: PartSource.Discovery );
        public static readonly Part InversionShield = new("Inversion Shield", shields: 2, energy: 2, source: PartSource.Discovery );
        public static readonly Part ShardHull = new("Shard Hull", hulls: 3, source: PartSource.Discovery );
        public static readonly Part ConformalDrive = new("Conformal Drive", movement: 4, initiative: 2, energy: -2, source: PartSource.Discovery );
        public static readonly Part NonLinearDrive = new("Nonlinear Drive", movement: 2, energy: 2, source: PartSource.Discovery );
        public static readonly Part HypergridSource = new("Hypergrid Source", energy: 11, source: PartSource.Discovery );

        // Esoteric parts
        public static readonly Part JumpDrive = new("Jump Drive", energy: -2, source: PartSource.Discovery);
        public static readonly Part MorphShield = new("Morph Shield", initiative: 1, shields: 1, source: PartSource.Discovery); // TODO: Implement
        public static readonly Part MuonSource = new("Muon Source", initiative: 1, energy: 2, source: PartSource.Discovery, outsideBlueprint: true);
    }
}
