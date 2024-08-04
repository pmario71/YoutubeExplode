using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YoutubeExplode.Demo.Cli.Utils;

public class ProfileHelper
{
    public static bool TryGetProfile(string destinationPath, out string? profileName)
    {
        var path = destinationPath.AsSpan().Trim();
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
        string configFileName = DetectConfigFile();
        var configObj = JsonSerializer.Deserialize(
            File.OpenRead("appsettings.json"),
            typeof(Config),
            SourceGenerationContext.Default
        );
        var cfg = configObj as Config;
        string? path = cfg?.Profiles.FirstOrDefault(f => f.Name == profileName)?.Path;
        return path;
    }

    public static string DetectConfigFile()
    {
        string? fname = Assembly.GetExecutingAssembly().GetName().Name;
        if (fname == null)
        {
            throw new Exception("No configuration found!");
        }
        fname = Path.ChangeExtension(fname, "json");
        string configFileName = Path.Combine(System.AppContext.BaseDirectory, fname);
        if (File.Exists(configFileName))
        {
            return configFileName;
        }
        
        configFileName = Path.Combine(System.AppContext.BaseDirectory, "appsettings.json");
        if (File.Exists(configFileName))
        {
            return configFileName;
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
