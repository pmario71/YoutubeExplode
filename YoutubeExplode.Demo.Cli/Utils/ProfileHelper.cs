using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YoutubeExplode.Demo.Cli.Utils;

public class ProfileHelper
{
    /// <summary>
    /// Checks if provided <see cref="profileOrPath"/> actually references a profile.
    /// If yes, it returns true an the corresponding profile name.
    /// </summary>
    /// <param name="profileOrPath"></param>
    /// <param name="profileName"></param>
    /// <returns>true, if profile is specified, false otherwise</returns>
    public static bool TryGetProfile(string profileOrPath, out string? profileName)
    {
        var path = profileOrPath.AsSpan().Trim();
        if (path[0] == '{' && path[^1] == '}')
        {
            profileName = path[1..^1].ToString();
            return true;
        }

        profileName = default;
        return false;
    }

    /// <summary>
    /// Resolves path for configured profile name.
    /// </summary>
    /// <param name="profileName"></param>
    /// <returns>path configured in profile</returns>
    public static string? ResolveProfilePath(string profileName)
    {
        Config? cfg = LoadConfig();
        string? path = cfg?.Profiles.FirstOrDefault(f => f.Name == profileName)?.Path;
        return path;
    }

    /// <summary>
    /// Loads configuration from configuration file (<executable filenam>.config)
    /// </summary>
    /// <returns></returns>
    public static Config? LoadConfig(bool verbose = false)
    {
        string configFileName = DetectConfigFile(verbose);
        var configObj = JsonSerializer.Deserialize(
            File.OpenRead(configFileName),
            typeof(Config),
            SourceGenerationContext.Default
        );
        return configObj as Config;
    }

    public static string DetectConfigFile(bool verbose = false)
    {
        if (verbose)
            Console.WriteLine("probing for:  <executable filename>.json");

        string? fname = "YoutubeExplode.json";
        string configFileName = Path.Combine(AppContext.BaseDirectory, fname);

        if (File.Exists(configFileName))
        {
            return configFileName;
        }
        else if (verbose)
        {
            Console.WriteLine($"Probing: {configFileName}");
        }

        if (verbose)
            Console.WriteLine("probing for:  appsettings.json");

        configFileName = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (File.Exists(configFileName))
        {
            return configFileName;
        }
        else if (verbose)
        {
            Console.WriteLine($"Probing: {configFileName}");
        }

        throw new Exception("No configuration found!");
    }
}

public record Config
{
    public required Profile[] Profiles { get; init; }
}

public record Profile(string Name, string Path);

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Config))]
[JsonSerializable(typeof(Profile[]))]
[JsonSerializable(typeof(Profile))]
internal partial class SourceGenerationContext : JsonSerializerContext { }
