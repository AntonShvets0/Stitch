using Spectre.Console;
using Stitch.Models;

namespace Stitch.Console;

public class SpectreConsoleRenderer : IConsoleRenderer
{
    private readonly IAnsiConsole _console;
    private readonly string _stitchArt = """

                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⠏⠀⠀⠀⠀⠀⠀⠀⣼⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⠀⠀⠀⠀⠀⠀⠀⢠⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀⠀⠀⠀⠀⠀⠀⡞⠸⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⣴⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⠤⠀
                                         ⠀⠀⠀⠀⠀⠀⢀⣶⠀⠀⠸⡄⠀⠀⠀⠀⠀⢰⠃⠀⡇⠀⠀⠀⢀⣤⣔⣚⣛⠛⠟⠛⠛⠉⠉⠙⠛⠻⢭⡿⣶⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡴⠋⠀⠀⡄
                                         ⠀⠀⣾⣦⣀⢰⡿⢿⡆⣀⣠⣷⡄⠀⠀⠀⠀⣼⠀⠀⢹⣀⣴⡿⠟⠋⠉⠉⠉⠛⠷⡄⠀⠀⠀⠀⠤⠤⢤⣙⢦⡉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡴⠋⠀⠀⠀⠀⠃
                                         ⠀⠀⠸⣅⠈⠛⠁⠈⠳⠏⢙⡇⠙⣦⣀⠀⠀⢿⠀⢀⡞⢡⠞⠀⠀⠀⠀⠀⠀⠀⠀⣷⠀⠀⠀⠀⠀⠀⠀⠀⠙⠳⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡰⠋⠀⠀⠀⠀⠀⢠⠀
                                         ⢠⣤⡤⡟⠀⡼⠋⢹⠀⠀⣿⠀⠀⠘⣎⠓⠦⢼⣤⠎⢠⡏⠀⣠⣾⣿⣿⢶⡄⠀⢀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⡄⠀⠀⠀⠀⠀⠀⠀⢀⡞⠁⠀⠀⠀⠀⠀⠀⡌⠀
                                         ⠀⠻⢧⣴⡀⠛⠦⠼⠂⠰⠛⠲⢤⡀⠘⢦⡀⢠⠏⠀⢸⠀⣰⣿⣿⣿⣇⣼⡇⠀⡾⠀⠀⠀⢀⠀⣀⠀⠀⠀⢀⣐⡲⣷⠀⠀⠀⠀⠀⠀⢠⠏⠀⠀⠀⠀⠀⠀⠀⣸⠁⠀
                                         ⠀⠀⠀⠈⠛⢶⠶⠚⠁⠀⠀⠀⠀⠉⠳⣄⠙⡿⠀⠀⠸⡄⢿⣿⣿⣿⣿⡿⠀⡼⢁⣀⣼⣗⣚⡯⣾⣗⠀⣠⠋⠉⠻⣿⡇⠀⠀⠀⠀⣰⠃⠀⠀⠀⠀⠀⠀⠀⡰⠃⠀⠀
                                         ⠀⠀⠀⠀⠀⢸⡇⠀⠀⠀⠀⠀⠀⠀⠀⠈⠳⡇⠴⢶⡆⠹⣌⣛⠿⢿⣿⡁⠞⣴⠋⠀⠀⠀⠈⠙⢾⣳⣤⠇⠀⠀⠀⢸⡇⠀⢀⡤⢺⠃⠀⠀⠀⠀⠀⠀⠀⡴⠃⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⢧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢳⠀⢸⡙⠢⣄⡉⢛⣺⠿⠃⢰⢷⡆⠀⠀⠀⠀⠀⠀⢹⣏⣴⣿⣿⣷⢸⣅⡴⠋⢠⠏⠀⠀⠀⠀⠀⠀⢀⡞⠁⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠈⢧⡀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣇⠀⢧⠀⣀⣹⣷⡦⣄⡀⠈⢻⣧⠀⠀⠀⠀⠀⣀⣿⣿⣿⣼⣿⡟⡼⠁⠀⢠⠏⠀⠀⠀⠀⠀⠀⣠⠏⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠹⣦⡀⠀⠀⠀⠀⠀⠀⠀⠘⣆⠈⢿⣡⡀⠀⠙⠶⠬⢿⠲⢯⣷⣤⣤⣶⠿⣿⠿⣿⣿⣿⡟⢱⠃⠀⡴⠋⠀⠀⠀⣄⣀⣠⠞⠁⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠙⠦⣄⡀⠸⡆⠀⠀⢀⣼⡷⣄⠙⢿⡒⣆⠀⠀⠘⣟⠒⠮⣝⠲⡦⣄⡘⢦⣉⣋⣉⡠⢋⣠⣞⠁⠀⠀⠀⣼⠁⠈⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠙⣇⠀⣰⢫⠎⠀⠈⠓⠦⣍⠻⠤⣄⣀⠈⠀⠀⣸⡆⠙⠒⣛⢶⣌⡽⠛⠛⠫⠦⠚⠛⠛⠋⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⣶⠇⡏⠀⠀⠀⢀⣤⠎⠉⠒⠦⣬⣉⠓⠛⠓⠛⢋⣭⣵⡊⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⢰⠇⠀⠀⢻⣷⠏⠀⠀⠀⠀⠀⢨⠿⡍⠉⠉⠉⠀⠀⠙⠲⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡼⢸⠀⠀⠀⢸⣏⠀⠀⠀⠀⠀⢰⠋⠀⢹⠀⠀⠀⠀⠀⠀⠀⠈⠳⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠇⢸⠀⠀⠀⠈⠛⠀⠀⠀⠀⠀⡏⠀⠀⠈⢧⣀⣠⠤⠒⠲⢤⡀⠀⠸⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠏⠀⡼⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⣤⠟⠁⠀⠀⠀⠀⠉⠀⠀⣷⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡟⠀⢀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢧⠀⠀⡼⠁⠀⠀⠀⠀⠀⠀⠀⠀⢠⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣆⠀⡇⠀⠀⠀⠀⠀⠀⠀⢀⣴⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⣆⢧⡀⠀⠀⠀⠤⢖⣚⣿⣾⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡾⣧⠀⢹⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⠀⠙⠦⣄⣀⡀⠈⠋⢈⣿⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⠏⠀⠘⣆⠘⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡾⠀⠀⠀⠀⠀⣯⣉⢉⢉⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀

                                         """;

    public SpectreConsoleRenderer(IAnsiConsole console = null)
    {
        _console = console ?? AnsiConsole.Console;
    }

    public void RenderWelcome()
    {
        _console.Clear();

        var banner = new Text(_stitchArt, new Style(Color.Blue));
        
        _console.Write(banner);
        _console.WriteLine();
    }

    public void RenderProgress<T>(string title, IEnumerable<T> items, Func<T, string> getDescription, Func<T, Task> processItem)
    {
        var itemList = items.ToList();
        
        _console.Progress()
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn(),
                new SpinnerColumn()
            })
            .Start(ctx =>
            {
                var task = ctx.AddTask(title, maxValue: itemList.Count);
                
                foreach (var item in itemList)
                {
                    task.Description = getDescription(item);
                    processItem(item).Wait();
                    task.Increment(1);
                }
            });
    }

    public void RenderSuccess(string message)
    {
        _console.MarkupLine($"[bold green]✓[/] {Markup.Escape(message)}");
    }

    public void RenderError(string message)
    {
        var panel = new Panel($"[red]{Markup.Escape(message)}[/]")
            .Header("[bold red]Error[/]")
            .BorderColor(Color.Red)
            .RoundedBorder();
        
        _console.Write(panel);
    }

    public void RenderInfo(string message)
    {
        _console.MarkupLine($"[bold blue]ℹ[/] {Markup.Escape(message)}");
    }

    public void RenderFileStatistics(IDictionary<string, FileStatistics> statistics, IEnumerable<string> files)
    {
        var table = new Table()
            .Title("[bold cyan]File Statistics[/]")
            .BorderColor(Color.Cyan1)
            .AddColumn("[bold]Extension[/]")
            .AddColumn("[bold]Files[/]", c => c.RightAligned())
            .AddColumn("[bold]Lines[/]", c => c.RightAligned())
            .AddColumn("[bold]Avg Lines[/]", c => c.RightAligned())
            .AddColumn("[bold]Size[/]", c => c.RightAligned());

        foreach (var stat in statistics.OrderByDescending(s => s.Value.TotalLines))
        {
            var avgLines = stat.Value.TotalLines / (double)stat.Value.FileCount;
            var size = stat.Value.Files.Sum(f => new FileInfo(f).Length);
            
            table.AddRow(
                $"[yellow]{stat.Key}[/]",
                stat.Value.FileCount.ToString(),
                stat.Value.TotalLines.ToString(),
                $"{avgLines:F1}",
                FormatFileSize(size)
            );
        }

        _console.Write(table);
        _console.WriteLine();

        // Summary
        var totalFiles = files.Count();
        var totalLines = statistics.Sum(s => s.Value.TotalLines);
        var summary = new Panel($"[bold green]Total: {totalFiles} files, {totalLines:N0} lines[/]")
            .BorderColor(Color.Green);
        
        _console.Write(summary);
        _console.WriteLine();

        // Top files
        RenderTopFiles(files);
    }

    public void RenderTable<T>(string title, IEnumerable<T> items, params (string Header, Func<T, string> ValueSelector)[] columns)
    {
        var table = new Table()
            .Title($"[bold]{title}[/]")
            .BorderColor(Color.Grey);

        foreach (var (header, _) in columns)
        {
            table.AddColumn($"[bold]{header}[/]");
        }

        foreach (var item in items)
        {
            var values = columns.Select(col => col.ValueSelector(item)).ToArray();
            table.AddRow(values);
        }

        _console.Write(table);
    }

    private void RenderTopFiles(IEnumerable<string> files)
    {
        var topFiles = files
            .Where(File.Exists)
            .Select(f => new { 
                File = f, 
                Lines = File.ReadLines(f).Count(),
                Name = Path.GetFileName(f)
            })
            .OrderByDescending(f => f.Lines)
            .Take(5);

        var table = new Table()
            .Title("[bold magenta]Top 5 Largest Files[/]")
            .BorderColor(Color.Magenta1)
            .AddColumn("[bold]File Name[/]")
            .AddColumn("[bold]Lines[/]", c => c.RightAligned());

        foreach (var file in topFiles)
        {
            table.AddRow(
                $"[white]{file.Name}[/]",
                $"[cyan]{file.Lines:N0}[/]"
            );
        }

        _console.Write(table);
    }

    private static string FormatFileSize(long bytes)
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
}