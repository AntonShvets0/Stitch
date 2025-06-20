using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Stitch.Configuration;

public static class ConfigurationManager
{
    private const string ConfigFileName = "appsettings.json";
    
    public static IConfiguration CreateConfiguration()
    {
        EnsureConfigFileExists();
        
        return new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(ConfigFileName, optional: false, reloadOnChange: true)
            .Build();
    }
    
    private static void EnsureConfigFileExists()
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
        
        if (!File.Exists(configPath))
        {
            System.Console.WriteLine($"File configuration {ConfigFileName} not found. Creating default configuration...");
            CreateDefaultConfigFile(configPath);
        }
    }
    
    private static void CreateDefaultConfigFile(string configPath)
    {
        var defaultConfig = new
        {
            Logging = new
            {
                LogLevel = new
                {
                    Default = "Information",
                    Microsoft = "Warning",
                    System = "Warning"
                }
            },
            AppSettings = new
            {
                MaxFileSize = 50 * 1024 * 1024,
                MaxTotalFiles = 1000,
                DefaultExtensions = new[] { "md" }
            }
        };
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var json = JsonSerializer.Serialize(defaultConfig, options);
        File.WriteAllText(configPath, json);
    }
}
