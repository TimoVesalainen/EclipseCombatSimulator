using EclipseCombatCalculatorLibrary.Dices;

namespace EclipseCombatCalculatorLibraryTest
{
    public class CombatTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private class TestShip : IShipStats
        {
            public int Initiative { get; set; }

            public Dice[]? Weapons { get; set; }

            public int Computers { get; set; }

            public int Shields { get; set; }

            public int Hulls { get; set; }

            public IEnumerable<Dice> Missiles => Array.Empty<Dice>();

            IEnumerable<Dice> IShipStats.Cannons => Weapons;
        }

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

            var result = await Combat.AttackerWin(
                new[] { (blueprint: new TestShip { Initiative = 1, Weapons = new Dice[] { CommonDices.YellowDice }, Computers = 0, Shields = 0, Hulls = 0 } as IShipStats, count: 1) },
                new[] { (blueprint: new TestShip { Initiative = 1, Weapons = new Dice[] { }, Computers = 0, Shields = 0, Hulls = 0 } as IShipStats, count: 1) },
                CombatAssingment);

            Assert.IsTrue(result);
        }
    }
}