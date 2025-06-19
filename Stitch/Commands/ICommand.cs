using Stitch.Models;

namespace Stitch.Commands;

public interface ICommand
{
    public void Execute(CommandOptions commandOptions);
}