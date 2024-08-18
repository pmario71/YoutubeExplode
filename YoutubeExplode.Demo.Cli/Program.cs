using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CommandLine;
using YoutubeExplode.Demo.Cli.Utils;
using YoutubeExplode.Demo.Cli.Verbs;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace YoutubeExplode.Demo.Cli;

public static class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DownloadVerb))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(InfoVerb))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TagLib.Mpeg4.File))]
    public static async Task Main(string[] args)
    {
        var a = Parser.Default.ParseArguments<DownloadVerb, InfoVerb>(args);

        await a.WithParsedAsync<DownloadVerb>(p => DownloadVerb.Download(p));
        await a.WithParsedAsync<InfoVerb>(p => InfoVerb.GetInfo(p));
    }
}
