namespace Stitch.Models;

public class StitchConfiguration
{
    public int MaxFileSize { get; set; }
    public int MaxTotalFiles { get; set; }
    public string[] DefaultExtensions { get; set; }
}