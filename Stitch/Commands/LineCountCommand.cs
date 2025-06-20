using Microsoft.Extensions.Options;
using Stitch.Console;
using Stitch.Models;
using Stitch.Services;
using Stitch.Services.Files;

namespace Stitch.Commands;

public class LineCountCommand : ICommand
{
    private readonly IConsoleRenderer _renderer;
    private readonly FileService _fileService;
    private readonly StitchConfiguration _stitchConfiguration;

    public LineCountCommand(IConsoleRenderer renderer, FileService fileService, IOptions<StitchConfiguration> stOptions)
    {
        _stitchConfiguration = stOptions.Value;
        _renderer = renderer;
        _fileService = fileService;
    }

    public async Task ExecuteAsync(CommandOptions options)
    {
        _renderer.RenderInfo($"Analyzing files matching '{string.Join(", ", options.Pattern)}'...");
        
        var result = await _fileService.GetFilesByPatternAsync(options.Pattern, options.IgnoreGitignore, _stitchConfiguration.DefaultExtensions);
        if (!result.IsSuccess)
        {
            _renderer.RenderError(result.Error);
            return;
        }

        var files = result.Value;
        if (!files.Any())
        {
            _renderer.RenderError($"No files found matching pattern '{string.Join(", ", options.Pattern)}'");
            return;
        }

        var statistics = await CalculateStatisticsAsync(files);
        _renderer.RenderFileStatistics(statistics, files);
    }

    private async Task<Dictionary<string, FileStatistics>> CalculateStatisticsAsync(IEnumerable<string> files)
    {
        var statistics = new Dictionary<string, FileStatistics>();

        await Task.Run(() =>
        {
            foreach (var file in files.Where(File.Exists))
            {
                var extension = Path.GetExtension(file).TrimStart('.').ToLowerInvariant();
                if (string.IsNullOrEmpty(extension)) extension = "no extension";

                if (!statistics.ContainsKey(extension))
                {
                    statistics[extension] = new FileStatistics
                    {
                        Extension = extension,
                        Files = new List<string>()
                    };
                }

                var lineCount = File.ReadLines(file).Count();
                statistics[extension].FileCount++;
                statistics[extension].TotalLines += lineCount;
                statistics[extension].Files.Add(file);
            }
        });

        return statistics;
    }
}