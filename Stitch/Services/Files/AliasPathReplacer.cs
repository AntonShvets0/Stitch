namespace Stitch.Services;

public class AliasPathReplacer
{
    private readonly Dictionary<string, string> _aliases;

    public AliasPathReplacer()
    {
        if (File.Exists("aliases.txt"))
        {
            _aliases = File
                .ReadAllLines("aliases.txt")
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