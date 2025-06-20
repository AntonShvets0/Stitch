using Spectre.Console;

namespace Stitch.Progress;

public class SpectreProgressTracker : IProgressTracker
{
    private readonly IAnsiConsole _console;

    public SpectreProgressTracker(IAnsiConsole console = null)
    {
        _console = console ?? AnsiConsole.Console;
    }

    public async Task TrackAsync<T>(string title, IEnumerable<T> items, Func<T, string> getDescription, Func<T, Task> processItem)
    {
        var itemList = items.ToList();
        
        await _console.Progress()
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn(),
                new SpinnerColumn()
            })
            .StartAsync(async ctx =>
            {
                var task = ctx.AddTask(title, maxValue: itemList.Count);
                
                foreach (var item in itemList)
                {
                    task.Description = getDescription(item);
                    await processItem(item);
                    task.Increment(1);
                }
            });
    }

    public async Task<TResult> TrackAsync<TResult>(string title, Func<ProgressContext, Task<TResult>> work)
    {
        return await _console.Progress()
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new SpinnerColumn()
            })
            .StartAsync(work);
    }
}