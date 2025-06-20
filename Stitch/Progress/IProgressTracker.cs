using Spectre.Console;

namespace Stitch.Progress;

public interface IProgressTracker
{
    Task TrackAsync<T>(string title, IEnumerable<T> items, Func<T, string> getDescription, Func<T, Task> processItem);
    Task<TResult> TrackAsync<TResult>(string title, Func<ProgressContext, Task<TResult>> work);
}