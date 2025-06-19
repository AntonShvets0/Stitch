using Microsoft.Extensions.DependencyInjection;
using Stitch.Models;

namespace Stitch.Commands;

public class CommandFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HashSet<CommandData> _commands;

    public CommandFactory(IServiceProvider serviceProvider, IEnumerable<CommandData> commands)
    {
        _serviceProvider = serviceProvider;
        _commands = commands.ToHashSet();
    }

    public IEnumerable<ICommand> CreateCommands(CommandOptions options)
    {
        var commands = _commands
            .Where(c => c.Pattern.Invoke(options));

        foreach (var data in commands)
        {
            yield return _serviceProvider.GetRequiredService(data.CommandType) as ICommand;
        }
    }
}