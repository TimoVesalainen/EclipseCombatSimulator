using EclipseCombatCalculatorLibrary;
using EclipseCombatCalculatorLibrary.Blueprints;
using EclipseCombatCalculatorLibrary.Dices;

namespace EclipseCombatCalculatorCommandLine
{
    internal static class RunCombat
    {
        public static async Task Run(Options options)
        {
            async Task<IEnumerable<(ICombatShip, IEnumerable<IDiceFace>)>> DamageAssigner(
                ICombatShip attacker, IEnumerable<ICombatShip> targets, IEnumerable<IDiceFace> diceResult)
            {
                if (true)
                {
                    return await AI.BasicAI(attacker, targets, diceResult);
                }

                //TODO: Ask human
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
