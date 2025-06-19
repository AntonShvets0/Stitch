namespace Stitch.Models;

public class FileStatistics
{
    public string Extension { get; set; }
    public int FileCount { get; set; }
    public int TotalLines { get; set; }
    public List<string> Files { get; set; } = new List<string>();
}