using Microsoft.Extensions.DependencyInjection;

namespace YoutubeExplode.Demo.Gui;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton(c => new ConfigurationReader(
            new ConfigReaderSettings { IgnoreHeader = true }
        ));
        collection.AddTransient<ViewModels.MainViewModel>();
    }
}
