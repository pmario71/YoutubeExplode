using CommandLine;
using YoutubeExplode.Demo.Cli.Utils;
using YoutubeExplode.Videos;

namespace YoutubeExplode.Demo.Cli.Verbs;

[Verb("info", aliases: ["i"], HelpText = "Display available muxed streams for youtube video!")]
public class InfoVerb
{
    // =================================================================
    [Option(
        longName: "url",
        shortName: 'u',
        Required = true,
        SetName = "videoinfo",
        HelpText = "video url."
    )]
    public string Url { get; set; } = string.Empty;

    [Option(
        longName: "profiles",
        shortName: 'p',
        Required = true,
        SetName = "profiles",
        HelpText = "list configured profiles."
    )]
    public bool Profiles { get; set; }

    internal static async Task GetInfo(InfoVerb args)
    {
        if (args.Profiles)
        {
            var config = ProfileHelper.LoadConfig();
            if (config == null)
            {
                System.Console.WriteLine("No config file found!");
                return;
            }
            if ((config.Profiles == null) || (!config.Profiles.Any()))
            {
                System.Console.WriteLine("No profiles configured!");
                return;
            }
            foreach (var profile in config.Profiles)
            {
                System.Console.WriteLine($"{profile.Name, 10}  -  {profile.Path}");
            }
        }
        else
        {
            var youtube = new YoutubeClient();

            // Get the video ID
            var videoId = VideoId.Parse(args.Url);

            // Get available streams and choose the best muxed (audio + video) stream
            var video = await youtube.Videos.GetAsync(videoId);
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest.GetMuxedStreams();

            if (!streamInfo.Any())
            {
                // Available streams vary depending on the video and it's possible
                // there may not be any muxed streams at all.
                Console.Error.WriteLine("This video has no muxed streams.");
                return;
            }

            string title = PathEx.SanitizeFileName(video.Title);
            foreach (var stream in streamInfo)
            {
                Console.WriteLine(
                    $"{title} - {stream.Container.Name}  -  {stream.VideoResolution}"
                );
            }
        }

        Console.WriteLine("Done");
    }
}
