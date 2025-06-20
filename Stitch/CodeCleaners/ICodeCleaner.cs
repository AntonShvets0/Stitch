namespace Stitch.CodeCleaners;

public interface ICodeCleaner
{
    public string Extension { get; set; }
    public string Clean(string code);
}