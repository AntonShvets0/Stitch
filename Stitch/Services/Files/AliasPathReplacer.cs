namespace Stitch.Services.Files;

public class AliasPathReplacer
{
    private readonly Dictionary<string, string> _aliases;

    public AliasPathReplacer()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aliases.txt");
        if (File.Exists(path))
        {
            _aliases = File
                .ReadAllLines(path)
                .Where(a => a.Contains('='))
                .Select(a => a.Split('='))
                .ToDictionary(a => a[0].Trim(), a => a[1].Trim());
            return;
        }
        
        _aliases = new Dictionary<string, string>();
    }

    public string ReplaceAliases(string path)
    {
        foreach (var (alias, replace) in _aliases)
        {
            path = path.Replace($"@{alias}", replace);
        }
        
        return path;
    }
}