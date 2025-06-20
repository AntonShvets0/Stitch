using CommandLine;

namespace Stitch.Models;

public class CommandOptions
{
    [Value(0, MetaName = "path", Required = true, HelpText = "Path to directory or template")]
    public IEnumerable<string> Pattern { get; set; }
    
    [Option('h', "help", Required = false, HelpText = "Show help information")]
    public bool ShowHelp { get; set; }

    [Option('l', "lines", Required = false, HelpText = "Show line count")]
    public bool ShowLinesCount { get; set; }
    
    [Option('c', "clipboard", Required = false, HelpText = "Copy to clipboard")]
    public bool CopyToClipboard { get; set; }
    
    [Option("ignore-gitignore", Required = false, HelpText = "Gitignore Pattern")]
    public bool IgnoreGitignore { get; set; }
        
    [Option("exclude-defaults", Required = false, HelpText = "Exclude default extension from appsettings.json")]
    public bool ExcludeDefaultExtensions { get; set; }

    [Option("clean", Required = false, HelpText = "Compress code")]
    public bool CleanCode { get; set; }
}