using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Gyan
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            using var loggerFactory = LoggerFactory.Create(config => config.AddConfiguration(configuration.GetSection("Logging")).AddConsole());
            var logger = loggerFactory.CreateLogger("Gyan");
            return await WireUpCommands(logger, configuration).InvokeAsync(args);
        }
        static RootCommand WireUpCommands(ILogger logger, IConfigurationRoot configuration)
        {
            var rootCommand = new RootCommand("Gyan")
            {
                new Option(new string[]{ "--help", "-h"}, "Help")
                {
                    Argument = new Argument<bool>()
                }
            };

            var addCommand = new Command("add", "Adds an article to Gyan");
            addCommand.AddArgument(new Argument("uri"));
            addCommand.Handler = CommandHandler.Create<Uri>(async (uri) =>
            {
                logger.LogInformation($"Add {uri}");
                await GyanConsoleUi.AddAsync(logger, configuration, uri);
            });
            rootCommand.AddCommand(addCommand);

            var removeCommand = new Command("remove", "Removes an article from Gyan");
            removeCommand.AddArgument(new Argument("uri"));
            removeCommand.Handler = CommandHandler.Create<Uri>(async (uri) =>
            {
                logger.LogInformation($"Remove {uri}");
                await GyanConsoleUi.RemoveAsync(logger, configuration, uri);
            });
            rootCommand.AddCommand(removeCommand);

            var listCommand = new Command("list", "List articles from Gyan");
            var filterArgument = new Argument("filter");
            filterArgument.SetDefaultValue("*");
            listCommand.AddArgument(filterArgument);
            listCommand.Handler = CommandHandler.Create<string>(async (filter) =>
            {
                logger.LogInformation($"List {filter}");
                await GyanConsoleUi.ListAsync(logger, configuration, filter);
            });
            rootCommand.AddCommand(listCommand);

            var analyzeCommand = new Command("analyze", "Analyze articles in Gyan");
            analyzeCommand.Handler = CommandHandler.Create(async () =>
            {
                logger.LogInformation($"Analyze");
                await GyanConsoleUi.AnalyzeAsync(logger, configuration);
            });
            rootCommand.AddCommand(analyzeCommand);

            rootCommand.Handler = CommandHandler.Create<bool>((help) =>
            {
                Console.WriteLine($"Value of Help {help}");
            });

            return rootCommand;
        }

    }
}
