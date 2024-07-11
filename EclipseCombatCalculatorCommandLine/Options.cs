using CommandLine;
using System;

namespace EclipseCombatCalculatorCommandLine
{
    public sealed class Options
    {
        [Option('r', Required = true, HelpText = "Are you attacker?")]
        public bool Attack { get; set; }

        [Option('a', Required = true, HelpText = "Attacker species")]
        public Species Attacker { get; set; }

        [Option('d', Required = true, HelpText = "Defender species")]
        public Species Defender { get; set; }

        [Option('s', Required = true, HelpText = "Attacker ships")]
        public int[] AttackerShipCounts { get; set; } = [];

        [Option('t', Required = true, HelpText = "Defender ships aka targets")]
        public int[] DefenderShipCounts { get; set; } = [];

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
