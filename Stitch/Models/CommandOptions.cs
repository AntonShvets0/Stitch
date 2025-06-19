using CommandLine;

namespace Stitch.Models;

public abstract class BaseCommandOptions;

public class ErrorCommandOptions : CommandOptions;
public class CommandOptions : BaseCommandOptions
{
    [Value(0, MetaName = "path", Required = true, HelpText = "Path to directory or template")]
    public string Path { get; set; }
}