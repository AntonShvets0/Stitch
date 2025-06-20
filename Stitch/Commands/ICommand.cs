using Stitch.Models;

namespace Stitch.Commands;

public interface ICommand
{
    public Task ExecuteAsync(CommandOptions commandOptions);
}