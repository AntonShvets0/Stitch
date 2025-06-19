using Microsoft.Extensions.DependencyInjection;
using Stitch.Commands;
using Stitch.Models;

namespace Stitch.Extensions;

public static class DependencyInjectionCommandExtensions
{
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