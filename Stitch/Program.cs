
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stitch.CodeCleaners;
using Stitch.Commands;
using Stitch.Console;
using Stitch.Extensions;
using Stitch.Models;
using Stitch.Services;
using Stitch.Services.Files;
using ConfigurationManager = Stitch.Configuration.ConfigurationManager;

var collection = new ServiceCollection();
var configuration = ConfigurationManager.CreateConfiguration();

// Config & Logging
collection.AddSingleton<IConfiguration>(configuration);
collection.Configure<StitchConfiguration>(options => 
    configuration.GetSection("AppSettings").Bind(options));
collection.AddLogging();

// Services
collection.AddConsoleServices();

collection.AddSingleton<FileService>();
collection.AddSingleton<CommandExecutor>();
collection.AddSingleton<CommandFactory>();
collection.AddSingleton<GitIgnoreChecker>();
collection.AddSingleton<AliasPathReplacer>();

// Cleaners
collection.AddSingleton<ICodeCleaner, CsharpCodeCleaner>();

// Commands
collection.RegisterCommand<LineCountCommand>(p => p.ShowLinesCount);
collection.RegisterCommand<BundleCommand>(_ => true);

var serviceProvider = collection.BuildServiceProvider();

var executor = serviceProvider.GetRequiredService<CommandExecutor>();
var consoleRenderer = serviceProvider.GetRequiredService<IConsoleRenderer>();

consoleRenderer.RenderWelcome();

await Parser.Default
    .ParseArguments<CommandOptions>(args)
    .WithParsedAsync(opt => executor.ExecuteAsync(opt));