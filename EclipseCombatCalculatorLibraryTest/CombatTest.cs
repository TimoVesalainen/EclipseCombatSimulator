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

            IEnumerable<Dice> IShipStats.Weapons => Weapons;
        }

        [Test]
        public void BasicCombatTest()
        {
            IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)> CombatAssingment(ICombatShip attacker, IEnumerable<ICombatShip> defenders, IEnumerable<IDiceFace> diceResult)
            {
                yield return (defenders.First(), diceResult);
            }

            var result = Combat.AttackerWin(
                new[] { (blueprint: new TestShip { Initiative = 1, Weapons = new Dice[] { Dices.YellowDice }, Computers = 0, Shields = 0, Hulls = 0 } as IShipStats, count: 1) }, 
                new[] { (blueprint: new TestShip { Initiative = 1, Weapons = new Dice[] { }, Computers = 0, Shields = 0, Hulls = 0 } as IShipStats, count: 1) },
                CombatAssingment);

            Assert.IsTrue(result);
        }
    }
}