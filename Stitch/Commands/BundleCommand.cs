using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stitch.Console;
using Stitch.Models;
using Stitch.Progress;
using Stitch.Services.Files;
using TextCopy;

namespace Stitch.Commands;

public class BundleCommand : ICommand
{
    private readonly IConsoleRenderer _renderer;
    private readonly IProgressTracker _progressTracker;
    private readonly FileService _fileService;
    private readonly ILogger<BundleCommand> _logger;
    private readonly StitchConfiguration _stitchConfiguration;
    private CancellationTokenSource _cancellationTokenSource;

    public BundleCommand(
        IConsoleRenderer renderer,
        IProgressTracker progressTracker,
        FileService fileService,
        ILogger<BundleCommand> logger,
        IOptions<StitchConfiguration> options)
    {
        _stitchConfiguration = options.Value;
        _renderer = renderer;
        _progressTracker = progressTracker;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task ExecuteAsync(CommandOptions options)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        System.Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            _cancellationTokenSource.Cancel();
            _renderer.RenderInfo("Operation cancelled by user");
        };

        try
        {
            var result = await _progressTracker.TrackAsync(
                "Processing files",
                async ctx =>
                {
                    var searchTask = ctx.AddTask("Searching for files...");
                    
                    _renderer.RenderInfo($"Searching for files matching '{string.Join(", ", options.Pattern)}'...");
                    
                    var filesResult = await _fileService.GetFilesByPatternAsync(
                        options.Pattern,
                        options.IgnoreGitignore,
                        _stitchConfiguration.DefaultExtensions,
                        new Progress<ProgressInfo>(info =>
                        {
                            var percentage = info.Total > 0 ? (double)info.Current / info.Total * 100 : 0;
                            searchTask.Description = $"{info.Message} ({percentage:F1}%)";
                            searchTask.Value = info.Current;
                            searchTask.MaxValue = info.Total;
                        }),
                        _cancellationTokenSource.Token);

                    if (!filesResult.IsSuccess)
                    {
                        _renderer.RenderError(filesResult.Error);
                        return string.Empty;
                    }

                    var files = filesResult.Value;
                    if (!files.Any())
                    {
                        _renderer.RenderError($"No files found matching pattern '{string.Join(", ", options.Pattern)}'");
                        return string.Empty;
                    }

                    searchTask.Description = "Search complete";
                    _renderer.RenderInfo($"Found {files.Count} file(s)");

                    var combineTask = ctx.AddTask("Combining files...");
                    var combineResult = await _fileService.CombineFilesAsync(
                        files,
                        options.CleanCode,
                        new Progress<ProgressInfo>(info =>
                        {
                            var percentage = info.Total > 0 ? (double)info.Current / info.Total * 100 : 0;
                            combineTask.Description = $"{info.Message} ({percentage:F1}%)";
                            combineTask.Value = info.Current;
                            combineTask.MaxValue = info.Total;
                        }),
                        _cancellationTokenSource.Token);

                    if (!combineResult.IsSuccess)
                    {
                        _renderer.RenderError(combineResult.Error);
                        return string.Empty;
                    }

                    combineTask.Description = "Files combined successfully";
                    return combineResult.Value;
                });

            if (string.IsNullOrEmpty(result))
                return;

            var savedPath = await SaveBundleAsync(result);
            _renderer.RenderSuccess($"Bundle saved to: {savedPath}");
            _renderer.RenderInfo($"Total size: {FormatFileSize(result.Length)}");

            if (options.CopyToClipboard)
            {
                ClipboardService.SetText(result);
                _renderer.RenderSuccess("Content copied to clipboard!");
            }
            else
            {
                _renderer.RenderInfo("Opening bundle folder...");
                OpenFolder(savedPath);
            }
        }
        catch (OperationCanceledException)
        {
            _renderer.RenderInfo("Operation was cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing bundle command");
            _renderer.RenderError($"Unexpected error: {ex.Message}");
        }
        finally
        {
            _cancellationTokenSource?.Dispose();
        }
    }

    private async Task<string> SaveBundleAsync(string content)
    {
        var date = DateTime.Now.ToString("yyyy-MM-dd");
        var bundleDir = Path.Combine("bundles", date);
        if (!Directory.Exists(bundleDir))
            Directory.CreateDirectory(bundleDir);

        var fileName = await GetNextFileNameAsync(bundleDir);
        await File.WriteAllTextAsync(fileName, content);
        return fileName;
    }

    private async Task<string> GetNextFileNameAsync(string bundleDir)
    {
        var files = await Task.Run(() => Directory.GetFiles(bundleDir, "*.txt"));
        var nextId = files.Length > 0
            ? files.Select(f => Path.GetFileNameWithoutExtension(f))
                   .Where(name => int.TryParse(name, out _))
                   .Select(int.Parse)
                   .DefaultIfEmpty(0)
                   .Max() + 1
            : 1;
        return Path.Combine(bundleDir, $"{nextId}.txt");
    }

    private string FormatFileSize(int bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }

    private void OpenFolder(string filePath)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            Process.Start(new ProcessStartInfo
            {
                FileName = directory,
                UseShellExecute = true,
                Verb = "open"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening folder");
            _renderer.RenderError("Could not open folder");
        }
    }
}
