using CommandLine;
using EclipseCombatCalculator.CommandLine;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var result = await Parser.Default.ParseArguments<Options>(args)
                   .WithParsedAsync(RunCombat.Run);

return result.Tag == ParserResultType.Parsed ? 0 : 1;