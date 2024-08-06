using EclipseCombatCalculator.Library;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.Library.Combat;
using EclipseCombatCalculator.Library.Dices;
using System;

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
            return $"{ship.Blueprint.Name} with {ship.Damage} damage";
        }

        private static IEnumerable<(ICombatShip, IEnumerable<DiceFace>)> PlayerDistribution(
            IShipStats activeShipBlueprint, bool isAttacker, IEnumerable<ICombatShip> targets, IEnumerable<DiceFace> diceResult)
        {
            Console.WriteLine("Your dice are: {0}", string.Join(", ", diceResult.Select(PrintDiceFace)));

            var mayHitDice = diceResult.Where(dice => targets.Any(target => activeShipBlueprint.CanHit(target.Blueprint, dice)));
            if (!mayHitDice.Any())
            {
                Console.WriteLine("No possible hits");
                return [];
            }

            Console.WriteLine("Please choose how to assign them. Possible targets are:");
            Console.WriteLine(string.Join("\n", targets.Select((target, i) => $"{i}) {PrintShip(target)}")));

            List<(DiceFace, ICombatShip)> results = [];
            var targetArray = targets.ToArray();
            var length = targetArray.Length;
            foreach (var dice in mayHitDice)
            {
                Console.Write("{0} => ", PrintDiceFace(dice));
                int? input = null;
                while (input == null)
                {
                    var read = Console.ReadLine();
                    if (read == null || read.Length == 0)
                    {
                        Console.WriteLine("Received empty input. Let's try that again, shall we?");
                        continue;
                    }
                    if (!int.TryParse(read, out var index))
                    {
                        Console.WriteLine("Not valid number. Let's try that again, shall we?");
                        continue;
                    }
                    if (index < 0 || index >= length)
                    {
                        Console.WriteLine("Invalid input. Let's try that again, shall we?");
                        continue;
                    }
                    input = index;
                }
                var ship = targetArray[input.Value];
                results.Add((dice, ship));
            }

            return results.GroupBy(resultPair => resultPair.Item2, resultPair => resultPair.Item1)
                .Select(group => (group.Key, group.Where(dice => activeShipBlueprint.CanHit(group.Key.Blueprint, dice))));
        }

        public static async Task Run(Options options)
        {
            async Task<IEnumerable<(ICombatShip, IEnumerable<DiceFace>)>> DamageAssigner(
                IShipStats activeShipBlueprint, bool isAttacker, IEnumerable<ICombatShip> targets, IEnumerable<DiceFace> diceResult)
            {
                if (options.Attack != isAttacker)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    Console.WriteLine("Enemy dice are: {0}", string.Join(", ", diceResult.Select(PrintDiceFace)));
                    var aiAssignment = await AI.BasicAI(activeShipBlueprint, isAttacker, targets, diceResult);
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
                    return PlayerDistribution(activeShipBlueprint, isAttacker, targets, diceResult);
                }
            }

            var attacker = Blueprint.GetBlueprints(options.Attacker).Cast<IShipStats>().Zip(options.AttackerShipCounts);
            var defender = Blueprint.GetBlueprints(options.Defender).Cast<IShipStats>().Zip(options.DefenderShipCounts);

            var run = await CombatLogic.AttackerWin(attacker, defender, DamageAssigner,
                (attacker, ships) => Task.FromResult(Enumerable.Empty<(ICombatShip ship, ShipCombatState newState)>()));

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
