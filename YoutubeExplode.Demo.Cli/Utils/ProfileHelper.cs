using Microsoft.Extensions.Configuration;

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

    public static string? ResolveProfilePath(IConfiguration cfg, string profileName)
    {
        var result = cfg.GetSection("Profiles")[profileName.ToString()];
        return result;
    }
}
