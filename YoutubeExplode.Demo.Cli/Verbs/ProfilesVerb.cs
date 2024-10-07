using System;
using CommandLine;
using YoutubeExplode.Demo.Cli.Utils;

namespace YoutubeExplode.Demo.Cli.Verbs;

[Verb(
    "profiles",
    aliases: ["p"],
    HelpText = "Display all configured profiles and storage locations!"
)]
public class ProfilesVerb
{
    internal static void GetProfiles()
    {
        Console.WriteLine();

        var profiles = ProfileHelper.LoadConfig();

        if (profiles == null || profiles.Profiles.Length == 0)
        {
            System.Console.WriteLine("No profiles configured in appsettings.json!");
        }
        else
        {
            Console.WriteLine($"Name               - Path");
            Console.WriteLine($"================================================================");

            foreach (var profile in profiles.Profiles)
            {
                Console.WriteLine($"{profile.Name:15} - {profile.Path}");
            }

            Console.WriteLine();
            Console.WriteLine($"profile config: {profiles.ProfileLocation}");
        }
    }
}
