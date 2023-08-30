using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace framework.Helper;

public static class ConfigManager
{
    public static ConcurrentDictionary<string, string?> Configurations = new();

    private static List<string> _configs = new()
    { "implicitWaitTimeout", "browser", "hubUrl", "useHub","environment","takeScreenshot","takeScreenshotOnlyforFailedStep" };

    public static void Configure()
    {
        // If already configured no need to call this again
        if (Configurations.Count > 0)
            return;

        try
        {
            IConfigurationRoot _settings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./testsettings.json")
            .Build();

            foreach (var config in _configs)
            {
                string? configValue = null;
                if (Environment.GetEnvironmentVariable(config.ToUpper()) != null) // Azure variables are added as environment variables in uppercase
                {
                    configValue = Environment.GetEnvironmentVariable(config.ToUpper());
                }
                else
                {
                    configValue = _settings[config];
                }
                _ = Configurations.TryAdd(config, configValue);
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error while fetching configurations", e);
        }
    }

    public static string? GetConfiguration(string configName)
    {
        Configurations.TryGetValue(configName, out var value);
        return value ?? string.Empty;
    }
}