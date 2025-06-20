using System.Text.RegularExpressions;

namespace Stitch.CodeCleaners;

public class CsharpCodeCleaner : ICodeCleaner
{
    public string Extension { get; set; } = "cs";

    public string Clean(string code)
    {
        if (string.IsNullOrEmpty(code))
            return code;

        var lines = code.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        var cleanedLines = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmedLine = line.Trim();

            // Пропускаем комментарии и пустые строки
            if (trimmedLine.StartsWith("//") || 
                trimmedLine.StartsWith("///") ||
                string.IsNullOrWhiteSpace(trimmedLine))
            {
                continue;
            }

            if (trimmedLine.StartsWith("[assembly:") || trimmedLine.StartsWith("global using"))
            {
                continue;
            }

            // Пропускаем using директивы
            if (trimmedLine.StartsWith("using ") && trimmedLine.EndsWith(";"))
            {
                continue;
            }

            // Обрабатываем namespace
            if (trimmedLine.StartsWith("namespace "))
            {
                continue;
            }

            var cleanedLine = CleanLine(line);
            if (!string.IsNullOrWhiteSpace(cleanedLine)) 
                cleanedLines.Add(cleanedLine);
        }

        var result = new List<string>();
        bool lastWasEmpty = false;
        foreach (var line in cleanedLines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (!lastWasEmpty)
                    result.Add("");
                lastWasEmpty = true;
            }
            else
            {
                result.Add(line);
                lastWasEmpty = false;
            }
        }

        while (result.Count > 0 && string.IsNullOrWhiteSpace(result[0]))
            result.RemoveAt(0);
        while (result.Count > 0 && string.IsNullOrWhiteSpace(result[result.Count - 1]))
            result.RemoveAt(result.Count - 1);

        return string.Join(Environment.NewLine, result);
    }

    private string CleanLine(string line)
    {
        var commentIndex = line.IndexOf("//");
        if (commentIndex >= 0)
        {
            // Проверяем, что // не внутри строки
            var beforeComment = line.Substring(0, commentIndex);
            var quotes = beforeComment.Count(c => c == '"') - beforeComment.Count(c => c == '\\' && beforeComment.IndexOf(c) < beforeComment.Length - 1 && beforeComment[beforeComment.IndexOf(c) + 1] == '"');
            if (quotes % 2 == 0) // четное количество кавычек означает, что мы не внутри строки
            {
                line = line.Substring(0, commentIndex);
            }
        }
        
        line = line.TrimEnd();
        line = line.Replace("string.Empty", "\"\"");
        line = Regex.Replace(line, @"\bthis\.", "");

        return line;
    }
}