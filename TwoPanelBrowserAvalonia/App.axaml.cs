using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication.ExtendedProtection;
using TwoPanelBrowserAvalonia.Controllers;

namespace TwoPanelBrowserAvalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = serviceProvider.GetService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<AppController>();
        services.AddTransient<MainWindow>();
    }
}