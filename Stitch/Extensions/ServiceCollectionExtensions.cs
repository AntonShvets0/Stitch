using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Stitch.Commands;
using Stitch.Console;
using Stitch.Models;
using Stitch.Progress;

namespace Stitch.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services)
    {
        services.AddSingleton<IAnsiConsole>(AnsiConsole.Console);
        services.AddSingleton<IConsoleRenderer, SpectreConsoleRenderer>();
        services.AddSingleton<IProgressTracker, SpectreProgressTracker>();
        return services;
    }

    public static IServiceCollection RegisterCommand<T>(this IServiceCollection serviceCollection,
        Predicate<CommandOptions> pattern
        )
        where T : class, ICommand
    {
        serviceCollection.AddSingleton(new CommandData(pattern, typeof(T)));
        serviceCollection.AddSingleton<T>();
        return serviceCollection;
    }
}