using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stitch.CodeCleaners;
using Stitch.Models;

namespace Stitch.Services.Files;

public class FileService
{
    private readonly ILogger<FileService> _logger;
    private readonly GitIgnoreChecker _gitIgnoreChecker;
    private readonly AliasPathReplacer _aliasPathReplacer;
    private readonly IEnumerable<ICodeCleaner> _codeCleaners;
    private readonly StitchConfiguration _config;
    
    public FileService(
        ILogger<FileService> logger,
        GitIgnoreChecker gitIgnoreChecker,
        AliasPathReplacer aliasPathReplacer,
        IEnumerable<ICodeCleaner> codeCleaners,
        IOptions<StitchConfiguration> config)
    {
        _logger = logger;
        _gitIgnoreChecker = gitIgnoreChecker;
        _aliasPathReplacer = aliasPathReplacer;
        _codeCleaners = codeCleaners;
        _config = config.Value;
    }
    
    public async Task<Result<List<string>>> GetFilesByPatternAsync(
        IEnumerable<string> patterns, 
        bool skipGitignore,
        string[] additionalExtensions,
        IProgress<ProgressInfo> progress = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var files = new List<string>();
            var patternList = patterns.ToList();

            foreach (var pattern in patterns)
            {
                foreach (var additionalExtension in additionalExtensions)
                {
                    var directory = 
                        !string.IsNullOrEmpty(Path.GetExtension(pattern)) ?
                        Path.GetDirectoryName(pattern) :
                        pattern;
                    if (string.IsNullOrEmpty(directory)) directory = ".";
                    patternList.Add(Path.Combine(directory, $"*.{additionalExtension}"));
                }   
            }
            
            for (int i = 0; i < patternList.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                var pattern = patternList[i];
                progress?.Report(new ProgressInfo($"Processing pattern: {pattern}", i, patternList.Count));
                
                var result = await ProcessPatternAsync(pattern, skipGitignore, cancellationToken);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to process pattern {Pattern}: {Error}", pattern, result.Error);
                    continue;
                }
                
                files.AddRange(result.Value);
            }
            
            if (files.Count > _config.MaxTotalFiles)
            {
                return Result<List<string>>.Failure(
                    $"Too many files found: {files.Count}. Maximum allowed: {_config.MaxTotalFiles}");
            }
            
            progress?.Report(new ProgressInfo("Complete", patternList.Count, patternList.Count));
            
            return Result<List<string>>.Success(files.Distinct().OrderBy(f => f).ToList());
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("File search operation was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting files by patterns");
            return Result<List<string>>.Failure($"Unexpected error: {ex.Message}");
        }
    }
    
    private async Task<Result<List<string>>> ProcessPatternAsync(
        string rawPattern, 
        bool skipGitignore, 
        CancellationToken cancellationToken)
    {
        try
        {
            var pattern = _aliasPathReplacer.ReplaceAliases(rawPattern);
            var directory = Path.GetDirectoryName(pattern);
            if (string.IsNullOrEmpty(directory)) directory = ".";
            
            if (!Directory.Exists(directory))
            {
                return Result<List<string>>.Failure($"Directory not found: {directory}");
            }
            
            var searchPattern = Path.GetFileName(pattern);
            if (string.IsNullOrEmpty(searchPattern)) return Result<List<string>>.Success([]);
            
            var files = Directory.EnumerateFiles(directory, searchPattern, SearchOption.AllDirectories);
            
            var validFiles = new List<string>();
            var gitignorePatterns = await GetGitignorePatternsAsync(directory, skipGitignore);
            
            foreach (var file in files)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                if (IsValidFile(file, gitignorePatterns, directory, skipGitignore))
                {
                    validFiles.Add(file);
                }
            }
            
            return Result<List<string>>.Success(validFiles);
        }
        catch (Exception ex)
        {
            return Result<List<string>>.Failure($"Error processing pattern {rawPattern}: {ex.Message}");
        }
    }
    
    public async Task<Result<string>> CombineFilesAsync(
        List<string> files, 
        bool cleanCode,
        IProgress<ProgressInfo> progress = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            const int maxConcurrency = 4;
            using var semaphore = new SemaphoreSlim(maxConcurrency);
    
            var tasks = files.Select(async (file, index) =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try
                {
                    progress?.Report(new ProgressInfo($"Processing: {Path.GetFileName(file)}", index, files.Count));
                    return await ProcessFileAsync(file, cleanCode, cancellationToken);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var results = await Task.WhenAll(tasks);
            progress?.Report(new ProgressInfo("Complete", files.Count, files.Count));
            return Result<string>.Success(string.Join("\n", results.Select(r => r.Value)));
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("File combination operation was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error combining files");
            return Result<string>.Failure($"Error combining files: {ex.Message}");
        }
    }
    
    private async Task<Result<string>> ProcessFileAsync(
        string filePath, 
        bool cleanCode, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Проверка размера файла
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length > _config.MaxFileSize)
            {
                return Result<string>.Failure($"File {filePath} is too large: {fileInfo.Length} bytes");
            }
            
            string content;
            await using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(fileStream))
            {
                content = await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            
            if (cleanCode)
            {
                var extension = Path.GetExtension(filePath).TrimStart('.');
                var cleaner = _codeCleaners.FirstOrDefault(c => c.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));
                if (cleaner != null)
                {
                    content = cleaner.Clean(content);
                }
            }
            
            return Result<string>.Success(content);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Error reading file {filePath}: {ex.Message}");
        }
    }
    
    private async Task<List<string>> GetGitignorePatternsAsync(string directory, bool skipGitignore)
    {
        if (skipGitignore) return new List<string>();
        
        var gitignorePath = Path.Combine(directory, ".gitignore");
        if (!File.Exists(gitignorePath)) return new List<string>();
        
        try
        {
            var lines = await File.ReadAllLinesAsync(gitignorePath);
            return ParseGitignore(lines);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error reading .gitignore file at {Path}", gitignorePath);
            return new List<string>();
        }
    }
    
    private List<string> ParseGitignore(string[] lines)
    {
        return lines
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith("#") && !line.StartsWith("!"))
            .ToList();
    }
    
    private bool IsValidFile(string file, List<string> gitignorePatterns, string directory, bool skipGitignore)
    {
        // Проверка на .gitignore
        if (!skipGitignore && gitignorePatterns.Any())
        {
            if (_gitIgnoreChecker.IsIgnored(file, gitignorePatterns, directory))
                return false;
        }
        
        return true;
    }
}