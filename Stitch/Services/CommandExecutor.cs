using Stitch.Commands;
using Stitch.Models;

namespace Stitch.Services;

public class CommandExecutor(CommandFactory commandFactory)
{
    public async Task ExecuteAsync(CommandOptions commandOptions)
    {
        await ExecuteCommandsAsync(commandFactory.CreateCommands(commandOptions), commandOptions);
    }

    private async Task ExecuteCommandsAsync(IEnumerable<ICommand> commands, CommandOptions options)
    {
        foreach (var command in commands)
            await command.ExecuteAsync(options);
    }
}