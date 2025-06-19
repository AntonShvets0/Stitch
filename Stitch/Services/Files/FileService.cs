using System.Diagnostics;
using System.Text;
using Stitch.Models;

namespace Stitch.Services;

public class FileService(GitIgnoreChecker gitIgnoreChecker, AliasPathReplacer aliasPathReplacer)
{
    public List<string> GetFilesByPattern(IEnumerable<string> patterns, bool skipGitignore)
    {
        var files = new List<string>();
        foreach (var pattern in patterns)
        {
            var directory = Path.GetDirectoryName(pattern);
            if (string.IsNullOrEmpty(directory))
                directory = ".";

            var gitignorePath = Path.Combine(directory, ".gitignore");
            var gitignorePatterns = !skipGitignore && File.Exists(gitignorePath)
                ? ParseGitignore(File.ReadAllLines(gitignorePath))
                : new List<string>();

            var searchPattern = Path.GetFileName(pattern);
            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*.*";

            var foundFiles = Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories)
                .OrderBy(f => f)
                .ToList();

            // Фильтруем файлы по gitignore паттернам
            if (!skipGitignore && gitignorePatterns.Any())
            {
                foundFiles = foundFiles
                    .Where(file => !gitIgnoreChecker.IsIgnored(file, gitignorePatterns, directory))
                    .ToList();
            }

            files.AddRange(foundFiles);
        }

        return files;
    }

    private List<string> ParseGitignore(string[] lines)
    {
        var patterns = new List<string>();

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // Пропускаем пустые строки и комментарии
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                continue;

            // Пропускаем негативные паттерны (начинающиеся с !)
            if (trimmed.StartsWith("!"))
                continue;

            patterns.Add(trimmed);
        }

        return patterns;
    }

    public string CombineFiles(List<string> files)
    {
        var combined = new StringBuilder();

        foreach (var file in files)
        {
            combined.AppendLine($"// ===== {Path.GetFileName(file)} =====");
            combined.AppendLine(File.ReadAllText(file));
            combined.AppendLine();
        }

        return combined.ToString();
    }

    public string SaveToBundle(string content)
    {
        var date = DateTime.Now.ToString("yyyy-MM-dd");
        var bundleDir = Path.Combine("bundles", date);

        if (!Directory.Exists(bundleDir))
            Directory.CreateDirectory(bundleDir);

        // Find next available ID
        var existingFiles = Directory.GetFiles(bundleDir, "*.txt");
        var nextId = 1;

        if (existingFiles.Length > 0)
        {
            var maxId = existingFiles
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .Where(name => int.TryParse(name, out _))
                .Select(int.Parse)
                .DefaultIfEmpty(0)
                .Max();
            nextId = maxId + 1;
        }

        var fileName = Path.Combine(bundleDir, $"{nextId}.txt");
        File.WriteAllText(fileName, content);

        return fileName;
    }

    public Dictionary<string, FileStatistics> GetFileStatistics(List<string> files)
    {
        var stats = new Dictionary<string, FileStatistics>();

        foreach (var file in files)
        {
            var extension = Path.GetExtension(file).ToLower();
            if (string.IsNullOrEmpty(extension))
                extension = "(no extension)";

            if (!stats.ContainsKey(extension))
            {
                stats[extension] = new FileStatistics { Extension = extension };
            }

            var lineCount = File.ReadLines(file).Count();
            stats[extension].FileCount++;
            stats[extension].TotalLines += lineCount;
            stats[extension].Files.Add(Path.GetFileName(file));
        }

        return stats;
    }

    public void OpenFolder(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        Process.Start(new ProcessStartInfo
        {
            FileName = directory,
            UseShellExecute = true,
            Verb = "open"
        });
    }
}