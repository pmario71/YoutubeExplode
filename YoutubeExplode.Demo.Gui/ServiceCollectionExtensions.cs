using Microsoft.Extensions.DependencyInjection;

namespace YoutubeExplode.Demo.Gui;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddTransient<ViewModels.MainViewModel>();
    }
}
