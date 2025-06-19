using Stitch.Models;
using Stitch.Services;

namespace Stitch.Commands;

public class HelpCommand(ConsoleService consoleService) : ICommand
{
    public void Execute(CommandOptions options)
    {
        consoleService.SetColor(ConsoleColor.Yellow);
        Console.WriteLine("Available Commands:");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        consoleService.ResetColor();

        consoleService.ShowCommand("stitch *.cs, *.js", "Combine files in folder and open result");
        consoleService.ShowCommand("stitch *.cs -c", "Combine files and copy to clipboard");
        consoleService.ShowCommand("stitch *.cs -l", "Show line count statistics by extension");
        consoleService.ShowCommand("stitch -h", "Show help message");

        Console.WriteLine();
        consoleService.SetColor(ConsoleColor.DarkGray);
        Console.WriteLine("Examples:");
        Console.WriteLine("  stitch *.txt");
        Console.WriteLine("  stitch src/**/*.cs -c");
        Console.WriteLine("  stitch *.* -l");
        consoleService.ResetColor();
    }
}