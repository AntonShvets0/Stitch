using Stitch.Models;

namespace Stitch.Console;

public interface IConsoleRenderer
{
    void RenderWelcome();
    void RenderProgress<T>(string title, IEnumerable<T> items, Func<T, string> getDescription, Func<T, Task> processItem);
    void RenderSuccess(string message);
    void RenderError(string message);
    void RenderInfo(string message);
    void RenderFileStatistics(IDictionary<string, FileStatistics> statistics, IEnumerable<string> files);
    void RenderTable<T>(string title, IEnumerable<T> items, params (string Header, Func<T, string> ValueSelector)[] columns);
}