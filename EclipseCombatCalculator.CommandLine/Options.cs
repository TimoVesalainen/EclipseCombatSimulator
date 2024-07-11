using CommandLine;
using System;

namespace EclipseCombatCalculator.CommandLine
{
    public sealed class Options
    {
        [Option('r', Required = true, HelpText = "Are you attacker?")]
        public bool Attack { get; set; }

        [Option('a', Required = true, HelpText = "Attacker species")]
        public Species Attacker { get; set; }

        [Option('d', Required = true, HelpText = "Defender species")]
        public Species Defender { get; set; }

        [Option('s', Separator = ',', Required = true, HelpText = "Attacker ships")]
        public IEnumerable<int> AttackerShipCounts { get; set; } = [];

        [Option('t', Separator = ',', Required = true, HelpText = "Defender ships aka targets")]
        public IEnumerable<int> DefenderShipCounts { get; set; } = [];

    }

    public enum Species
    {
        Terran,

        Planta,
        Draco,
        Orion,
        Mechamena,
        Eridani,
        Hydran,

        Magellan,
        Lyra,
        Exiles,
        RhoIndi,
    }

}
