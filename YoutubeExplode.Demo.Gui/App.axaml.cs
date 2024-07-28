using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Material.Styles.Themes;
using Microsoft.Extensions.DependencyInjection;
using YoutubeExplode.Demo.Gui.Views;

namespace YoutubeExplode.Demo.Gui;

public class App : Application
{
    public override void Initialize()
    {
        base.Initialize();

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            desktopLifetime.MainWindow = new MainWindow();

        // Register all the services needed for the application to run
        var collection = new ServiceCollection();
        collection.AddCommonServices();

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();

        var vm = services.GetRequiredService<ViewModels.MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow { DataContext = vm };
        }

        base.OnFrameworkInitializationCompleted();

        // Set up custom theme colors
        this.LocateMaterialTheme<MaterialThemeBase>().CurrentTheme = Theme.Create(
            Theme.Light,
            Color.Parse("#343838"),
            Color.Parse("#F9A825")
        );
    }
}
