
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Stitch.Commands;
using Stitch.Extensions;
using Stitch.Models;
using Stitch.Services;
using Stitch.Services.Files;

var collection = new ServiceCollection();

collection.AddSingleton<ConsoleService>();
collection.AddSingleton<FileService>();
collection.AddSingleton<CommandExecutor>();
collection.AddSingleton<CommandFactory>();
collection.AddSingleton<GitIgnoreChecker>();
collection.AddSingleton<AliasPathReplacer>();

collection.RegisterCommand<HelpCommand>(p => p.ShowHelp);
collection.RegisterCommand<LineCountCommand>(p => p.ShowLinesCount);
collection.RegisterCommand<BundleCommand>(p => !p.ShowHelp);

var serviceProvider = collection.BuildServiceProvider();

var executor = serviceProvider.GetRequiredService<CommandExecutor>();
var consoleService = serviceProvider.GetRequiredService<ConsoleService>();

consoleService.ShowWelcome();

Parser.Default.ParseArguments<CommandOptions>(args)
    .WithParsed(opt => executor.Execute(opt))
    .WithNotParsed(opt => executor.Execute(null));