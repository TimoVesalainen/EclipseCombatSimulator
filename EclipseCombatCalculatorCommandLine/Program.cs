using CommandLine;
using EclipseCombatCalculatorCommandLine;

var result = await Parser.Default.ParseArguments<Options>(args)
                   .WithParsedAsync(RunCombat.Run);

var result2 = await result.WithNotParsedAsync(async errors => {
    Console.WriteLine("Errors parsing command line arguments");
    foreach (var error in errors)
    {
        Console.WriteLine(" - {0}", error);
    }
    Console.WriteLine("Please fix these errors");
});

return result.Tag == ParserResultType.Parsed ? 0 : 1;