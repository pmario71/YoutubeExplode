using System;
using CommandLine;
using YoutubeExplode.Demo.Cli.Utils;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace YoutubeExplode.Demo.Cli.Verbs;

[Verb("download", aliases: ["d"], HelpText = "Download youtube video for url!")]
public class DownloadVerb
{
    // =================================================================
    [Option(longName: "url", shortName: 'u', Required = true, HelpText = "video url.")]
    public string Url { get; set; } = string.Empty;

    // =================================================================
    [Option(
        longName: "out",
        shortName: 'o',
        Required = false,
        HelpText = "path to save video file to. Use curly braces to resolve path from appSettings.json: '{profilename}'"
    )]
    public string DestinationPath { get; set; } = Environment.CurrentDirectory;

    internal static async Task Download(DownloadVerb args)
    {
        Console.Title = "YoutubeExplode Demo";

        string destinationPath = args.DestinationPath;

        var profileOption = ProfileHelper.TryGetProfile(destinationPath, out var profileName);
        if (profileOption)
        {
            var resolvedPath = ProfileHelper.ResolveProfilePath(profileName!);
            if (resolvedPath == null)
            {
                Console.Error.WriteLine($"No profile with name '{profileName}'");
                return;
            }
            destinationPath = resolvedPath;
        }

        var youtube = new YoutubeClient();

        // Get the video ID
        var videoId = VideoId.Parse(args.Url);

        // Get available streams and choose the best muxed (audio + video) stream
        var video = await youtube.Videos.GetAsync(videoId);
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
        var streamInfo = streamManifest.GetMuxedStreams().TryGetWithHighestVideoQuality();
        if (streamInfo is null)
        {
            // Available streams vary depending on the video and it's possible
            // there may not be any muxed streams at all.
            // See the readme to learn how to handle adaptive streams.
            Console.Error.WriteLine("This video has no muxed streams.");
            return;
        }

        // Download the stream
        string title = PathEx.SanitizeFileName(video.Title);
        var fileName = Path.Combine(destinationPath, title) + $".{streamInfo.Container.Name}";

        Console.Write(
            $"Downloading stream: {streamInfo.VideoQuality.Label} / {streamInfo.Container.Name} ... "
        );

        using (var progress = new ConsoleProgress())
            await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName, progress);

        Console.WriteLine("Done");
        Console.WriteLine($"Video saved to '{fileName}'");
    }
}
