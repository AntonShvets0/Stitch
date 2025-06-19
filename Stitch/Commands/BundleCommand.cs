using Stitch.Models;
using Stitch.Services;
using Stitch.Services.Files;
using TextCopy;

namespace Stitch.Commands;

public class BundleCommand(ConsoleService consoleService, FileService fileService) : ICommand
{
    public void Execute(CommandOptions options)
    {
        consoleService.ShowProgress($"Searching for files matching '{string.Join(", ", options.Pattern)}'...");
        
        var files = fileService.GetFilesByPattern(options.Pattern, options.IgnoreGitignore);
        consoleService.ClearProgress();

        if (!files.Any())
        {
            consoleService.ShowError($"No files found matching pattern '{string.Join(", ", options.Pattern)}'");
            return;
        }

        consoleService.ShowInfo($"Found {files.Count} file(s)");

        consoleService.ShowProgress("Combining files...");
        var combined = fileService.CombineFiles(files);
        consoleService.ClearProgress();

        var savedPath = fileService.SaveToBundle(combined);
        consoleService.ShowSuccess($"Bundle saved to: {savedPath}");
        consoleService.ShowInfo($"Total size: {FormatFileSize(combined.Length)}");

        if (options.CopyToClipboard)
        {
            ClipboardService.SetText(combined);
            consoleService.ShowSuccess("Content copied to clipboard!");
        }
        else
        {
            consoleService.ShowInfo("Opening bundle folder...");
            fileService.OpenFolder(savedPath);
        }
    }

    private string FormatFileSize(int bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}
