using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library.Combat;
using EclipseCombatCalculator.Library.Dices;

namespace EclipseCombatCalculator.Library.Test
{
    public class CombatTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private class TestShip : IShipStats
        {
            public string Name => "Test";
            public int Initiative { get; set; }

            public Dice[]? Weapons { get; set; }

            public int Computers { get; set; }

            public int Shields { get; set; }

            public int Hulls { get; set; }

            public int Size { get; set; } = 0;

            public IEnumerable<Dice> Missiles => Array.Empty<Dice>();

            public ShipType ShipType { get; set; }

            IEnumerable<Dice> IShipStats.Cannons => Weapons ?? Array.Empty<Dice>();
        }

        static readonly RetreatAsker NoRetreat = (ICombatShip ship) => Task.FromResult((0, 0));

        [Test]
        public async Task BasicCombatTest()
        {
            async Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> CombatAssingment(ICombatShip attacker, IEnumerable<ICombatShip> defenders, IEnumerable<IDiceFace> diceResult)
            {
                IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)> Result()
                {
                    yield return (defenders.First(), diceResult);
                }

                return Result();
            }

            var result = await CombatLogic.AttackerWin(
                new[] { (blueprint: new TestShip { Initiative = 1, Weapons = new Dice[] { CommonDices.YellowDice }, Computers = 0, Shields = 0, Hulls = 0 } as IShipStats, count: 1) },
                new[] { (blueprint: new TestShip { Initiative = 1, Weapons = Array.Empty<Dice>(), Computers = 0, Shields = 0, Hulls = 0 } as IShipStats, count: 1) },
                CombatAssingment, NoRetreat);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DefaultPlantaVSOrion()
        {
            async Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> CombatAssignment(ICombatShip attacker, IEnumerable<ICombatShip> defenders, IEnumerable<IDiceFace> diceResult)
            {
                return defenders.Zip(diceResult, (x, y) => (x, new[] { y } as IEnumerable<IDiceFace>));
            }

            var result = await CombatLogic.AttackerWin(
                new[] { (blueprint: Blueprint.OrionInterceptor as IShipStats, count: 5) },
                new[] { (blueprint: Blueprint.PlantaInterceptor as IShipStats, count: 1) },
                CombatAssignment, NoRetreat);
        }

        [Test]
        public async Task DefaultAIPlantaVSOrion()
        {
            var result = await CombatLogic.AttackerWin(
                new[] { (blueprint: Blueprint.OrionInterceptor as IShipStats, count: 5) },
                new[] { (blueprint: Blueprint.PlantaInterceptor as IShipStats, count: 1) },
                AI.BasicAI, NoRetreat);
        }
    }
}