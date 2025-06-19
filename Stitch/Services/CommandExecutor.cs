using Stitch.Commands;
using Stitch.Models;

namespace Stitch.Services;

public class CommandExecutor(CommandFactory commandFactory)
{
    public void Execute(CommandOptions? commandOptions)
    {
        if (commandOptions == null)
            commandOptions = new CommandOptions
            {
                ShowHelp = true
            };
        
        ExecuteCommands(commandFactory.CreateCommands(commandOptions), commandOptions);
    }

    private void ExecuteCommands(IEnumerable<ICommand> commands, CommandOptions options)
    {
        foreach (var command in commands)
            command.Execute(options);
    }
}