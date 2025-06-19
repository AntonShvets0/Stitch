using System.Text.RegularExpressions;

namespace Stitch.Services;

public class GitIgnoreChecker
{
    public bool IsIgnored(string filePath, List<string> gitignorePatterns, string baseDirectory)
    {
        var relativePath = Path.GetRelativePath(baseDirectory, filePath);

        // Нормализуем слеши для кроссплатформенности
        relativePath = relativePath.Replace('\\', '/');

        foreach (var pattern in gitignorePatterns)
        {
            if (MatchesGitignorePattern(relativePath, pattern))
                return true;
        }

        return false;
    }

    private bool MatchesGitignorePattern(string filePath, string pattern)
    {
        pattern = pattern.Trim('/');

        // Если паттерн содержит /, то это путь от корня
        if (pattern.Contains('/'))
        {
            return MatchesPattern(filePath, pattern);
        }
        
        var fileName = Path.GetFileName(filePath);
        var pathParts = filePath.Split('/');

        // Проверяем имя файла
        if (MatchesPattern(fileName, pattern))
            return true;

        // Проверяем каждую часть пути
        foreach (var part in pathParts)
        {
            if (MatchesPattern(part, pattern))
                return true;
        }

        return false;
    }

    private bool MatchesPattern(string text, string pattern)
    {
        var regexPattern = "^" + Regex.Escape(pattern)
                                   .Replace("\\*\\*", ".*") // ** означает любые директории
                                   .Replace("\\*", "[^/]*") // * означает любые символы кроме /
                                   .Replace("\\?", "[^/]") // ? означает один символ кроме /
                               + "$";

        return Regex.IsMatch(text, regexPattern, RegexOptions.IgnoreCase);
    }
}