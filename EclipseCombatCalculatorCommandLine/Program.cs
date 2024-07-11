using CommandLine;
using EclipseCombatCalculatorCommandLine;

var result = await Parser.Default.ParseArguments<Options>(args)
                   .WithParsedAsync(RunCombat.Run);

return result.Tag == ParserResultType.Parsed ? 0 : 1;