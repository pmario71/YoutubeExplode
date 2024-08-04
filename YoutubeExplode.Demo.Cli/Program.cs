using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CommandLine;
// using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using YoutubeExplode.Demo.Cli.Utils;
using YoutubeExplode.Demo.Cli.Verbs;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace YoutubeExplode.Demo.Cli;

public static class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DownloadVerb))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(InfoVerb))]
    public static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // //setup our DI
        // var serviceCollection = new ServiceCollection()
        //     .AddSingleton<IConfiguration>(configuration);
        // var serviceProvider = serviceCollection.BuildServiceProvider();

        var a = Parser.Default.ParseArguments<DownloadVerb, InfoVerb>(args);

        await a.WithParsedAsync<DownloadVerb>(p => DownloadVerb.Download(configuration, p));
        await a.WithParsedAsync<InfoVerb>(p => InfoVerb.GetInfo(p));
    }
}
