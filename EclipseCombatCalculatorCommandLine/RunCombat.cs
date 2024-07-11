using EclipseCombatCalculatorLibrary;
using EclipseCombatCalculatorLibrary.Blueprints;
using EclipseCombatCalculatorLibrary.Dices;

namespace EclipseCombatCalculatorCommandLine
{
    internal static class RunCombat
    {
        private static string PrintDiceFace(IDiceFace dice)
        {
            return dice switch
            {
                Damage damage => string.Join("", Enumerable.Range(0, damage.DamageToOpponent).Select(_ => "*")),
                Number number => $"{number.Value}({string.Join("", Enumerable.Range(0, number.DamageToOpponent).Select(_ => "*"))})",
                Miss number => "_",
                _ => throw new NotImplementedException(),
            };
        }

        private static string PrintShip(ICombatShip ship)
        {
            return $"{ship.Blueprint.Name} amount {ship.Count} with {ship.Damage} damage";
        }

        private static IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)> PlayerDistribution(
            ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult)
        {
            Console.WriteLine("Your dice are: {0}", string.Join(", ", diceResult.Select(PrintDiceFace)));

            var mayHitDice = diceResult.Where(dice => dice is not Miss && targets.Any(target => attacker.Blueprint.CanHit(target.Blueprint, dice)));
            if (!mayHitDice.Any())
            {
                Console.WriteLine("No possible hits");
                return [];
            }

            Console.WriteLine("Please choose how to assign them. Possible targets are:");
            Console.WriteLine(string.Join("\n", targets.Zip(Options).Select((pair) => $"{pair.Second}) {PrintShip(pair.First)}")));

            List<(IDiceFace, ICombatShip)> results = [];
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
            async Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> DamageAssigner(
                ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult)
            {
                if (options.Attack != attacker.Attacker)
                {
                    Console.WriteLine("AI are: {0}", string.Join(", ", diceResult.Select(PrintDiceFace)));
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

            var attacker = GetBlueprints(options.Attacker).Zip(options.AttackerShipCounts).Where(x => x.First != null);
            var defender = GetBlueprints(options.Defender).Zip(options.DefenderShipCounts).Where(x => x.First != null);

            var run = await Combat.AttackerWin(attacker, defender, DamageAssigner);

            if (run)
            {
                Console.WriteLine("You win");
            }
            else
            {
                Console.WriteLine("You lose");
            }
        }

        private static IShipStats?[] GetBlueprints(Species species)
        {
            return species switch
            {
                Species.Planta => [
                        Blueprint.PlantaInterceptor,
                        Blueprint.PlantaCruiser,
                        Blueprint.PlantaDreadnaught,
                        Blueprint.PlantaStarbase],
                Species.Orion => [
                        Blueprint.OrionInterceptor,
                        Blueprint.OrionCruiser,
                        Blueprint.OrionDreadnaught,
                        Blueprint.OrionStarbase],
                Species.Eridani => [
                        Blueprint.EridaniInterceptor,
                        Blueprint.EridaniCruiser,
                        Blueprint.EridaniDreadnaught,
                        Blueprint.EridaniStarbase],
                Species.Exiles => [
                        Blueprint.ExilesInterceptor,
                        Blueprint.ExilesCruiser,
                        Blueprint.ExilesDreadnaught,
                        Blueprint.ExilesOrbital],
                Species.RhoIndi => [
                        Blueprint.RhoIndiInterceptor,
                        Blueprint.RhoIndiCruiser,
                        null,
                        Blueprint.RhoIndiStarbase],
                Species.Terran or Species.Hydran or Species.Draco or Species.Mechamena or Species.Magellan or Species.Lyra => [
                        Blueprint.TerranInterceptor,
                        Blueprint.TerranCruiser,
                        Blueprint.TerranDreadnaught,
                        Blueprint.TerranStarbase],
                _ => throw new NotImplementedException(),
            };
        }
    }
}
