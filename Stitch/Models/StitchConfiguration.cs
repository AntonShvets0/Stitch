namespace Stitch.Models;

public class StitchConfiguration
{
    public string BundleDirectory { get; set; }
    public int MaxFileSize { get; set; }
    public int MaxTotalFiles { get; set; }
    public string[] DefaultExtensions { get; set; }
}