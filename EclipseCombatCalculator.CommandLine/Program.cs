using CommandLine;
using EclipseCombatCalculator.CommandLine;

var result = await Parser.Default.ParseArguments<Options>(args)
                   .WithParsedAsync(RunCombat.Run);

return result.Tag == ParserResultType.Parsed ? 0 : 1;