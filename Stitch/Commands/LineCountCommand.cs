using Stitch.Models;
using Stitch.Services;
using Stitch.Services.Files;

namespace Stitch.Commands;

public class LineCountCommand(ConsoleService consoleService, FileService fileService) : ICommand
{
    public void Execute(CommandOptions options)
    {
        consoleService.ShowProgress($"Analyzing files matching '{options.Pattern}'...");
        
        var files = fileService.GetFilesByPattern(options.Pattern, options.IgnoreGitignore);
        consoleService.ClearProgress();

        if (!files.Any())
        {
            consoleService.ShowError($"No files found matching pattern '{options.Pattern}'");
            return;
        }

        var statistics = fileService.GetFileStatistics(files);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("File Statistics");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{"Extension",-15} {"Files",-10} {"Lines",-10} {"Avg Lines",-10}");
        Console.WriteLine($"{new string('─', 15)} {new string('─', 10)} {new string('─', 10)} {new string('─', 10)}");
        Console.ResetColor();

        foreach (var stat in statistics.OrderByDescending(s => s.Value.TotalLines))
        {
            var avgLines = stat.Value.TotalLines / (double)stat.Value.FileCount;
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{stat.Key,-15} {stat.Value.FileCount,-10} {stat.Value.TotalLines,-10} {avgLines,-10:F1}");
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Total: {files.Count} files, {statistics.Sum(s => s.Value.TotalLines)} lines");
        Console.ResetColor();

        // Show top 5 largest files
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Top 5 Largest Files:");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.ResetColor();

        var topFiles = files
            .Select(f => new { File = f, Lines = System.IO.File.ReadLines(f).Count() })
            .OrderByDescending(f => f.Lines)
            .Take(5);

        foreach (var file in topFiles)
        {
            Console.WriteLine($"  {System.IO.Path.GetFileName(file.File),-40} {file.Lines,6} lines");
        }
    }
}