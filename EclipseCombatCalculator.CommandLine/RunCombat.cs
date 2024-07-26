using EclipseCombatCalculator.Library;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library.Combat;
using EclipseCombatCalculator.Library.Dices;

namespace EclipseCombatCalculator.CommandLine
{
    internal static class RunCombat
    {
        private static string PrintDiceFace(DiceFace dice)
        {
            var damage = string.Join("", Enumerable.Range(0, dice.DamageToOpponent).Select(_ => "★")) +
                string.Join("", Enumerable.Range(0, dice.DamageToSelf).Select(_ => "☆"));
            if (dice.Number == null)
            {
                if (damage.Length == 0)
                {
                    return "_";
                }
                return damage;
            }
            return $"{dice.Number}({damage})";
        }

        private static string PrintShip(ICombatShip ship)
        {
            return $"{ship.Blueprint.Name} amount {ship.InCombat} with {ship.Damage} damage";
        }

        private static IEnumerable<(ICombatShip, IEnumerable<DiceFace>)> PlayerDistribution(
            ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<DiceFace> diceResult)
        {
            Console.WriteLine("Your dice are: {0}", string.Join(", ", diceResult.Select(PrintDiceFace)));

            var mayHitDice = diceResult.Where(dice => targets.Any(target => attacker.Blueprint.CanHit(target.Blueprint, dice)));
            if (!mayHitDice.Any())
            {
                Console.WriteLine("No possible hits");
                return [];
            }

            Console.WriteLine("Please choose how to assign them. Possible targets are:");
            Console.WriteLine(string.Join("\n", targets.Zip(Options).Select((pair) => $"{pair.Second}) {PrintShip(pair.First)}")));

            List<(DiceFace, ICombatShip)> results = [];
            var targetArray = targets.ToArray();
            var length = targetArray.Length;
            foreach (var dice in mayHitDice)
            {
                Console.Write("{0} => ", PrintDiceFace(dice));
                char? input = null;
                while (input == null)
                {
                    var read = Console.ReadLine();
                    if (read == null || read.Length == 0)
                    {
                        Console.WriteLine("Received empty input. Let's try that again, shall we?");
                        continue;
                    }
                    var index = Options.IndexOf(read[0]);
                    if (index < 0 || index >= length)
                    {
                        Console.WriteLine("Invalid input. Let's try that again, shall we?");
                        continue;
                    }
                    input = read[0];
                }
                var ship = targetArray[Options.IndexOf(input ?? ' ')];
                results.Add((dice, ship));
            }

            return results.GroupBy(resultPair => resultPair.Item2, resultPair => resultPair.Item1)
                .Select(group => (group.Key, group.Where(dice => attacker.Blueprint.CanHit(group.Key.Blueprint, dice))));
        }

        const string Options = "abcd";

        public static async Task Run(Options options)
        {
            async Task<IEnumerable<(ICombatShip, IEnumerable<DiceFace>)>> DamageAssigner(
                ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<DiceFace> diceResult)
            {
                if (options.Attack != attacker.IsAttacker)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    Console.WriteLine("Enemy dice are: {0}", string.Join(", ", diceResult.Select(PrintDiceFace)));
                    var aiAssignment = await AI.BasicAI(attacker, targets, diceResult);
                    if (aiAssignment.Any())
                    {
                        Console.WriteLine("Assigns {0}", string.Join(", ", aiAssignment.Select(x => $"{string.Join(", ", x.Item2.Select(PrintDiceFace))} -> to ship {PrintShip(x.Item1)}")));
                    }
                    else
                    {
                        Console.WriteLine("No hits");
                    }
                    return aiAssignment;
                }
                else
                {
                    return PlayerDistribution(attacker, targets, diceResult);
                }
            }

            var attacker = Blueprint.GetBlueprints(options.Attacker).Cast<IShipStats>().Zip(options.AttackerShipCounts);
            var defender = Blueprint.GetBlueprints(options.Defender).Cast<IShipStats>().Zip(options.DefenderShipCounts);

            var run = await CombatLogic.AttackerWin(attacker, defender, DamageAssigner, (type) => Task.FromResult((0,0)));

            if (run)
            {
                Console.WriteLine("You win");
            }
            else
            {
                Console.WriteLine("You lose");
            }
        }
    }
}
