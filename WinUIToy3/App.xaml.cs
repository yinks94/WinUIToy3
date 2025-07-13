using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using WinUIEx;
using WinUIToy3.Contracts.Services;
using WinUIToy3.Core.Contracts.Services;
using WinUIToy3.Core.Services;
using WinUIToy3.Services;
using WinUIToy3.ViewModels;
using WinUIToy3.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIToy3;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    public IHost Host
    {
        get;
    }

    public static T GetService<T>() where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new InvalidOperationException($"Service of type {typeof(T).FullName} is not registered.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();

        Host = Microsoft
                .Extensions
                .Hosting
                .Host
                .CreateDefaultBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureServices((context, services) =>
                {
                    // Register application services here

                    // Singleton services are created once and shared throughout the application lifetime.
                    services.AddSingleton<IActivationService, ActivationService>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<IPageService, PageService>();
                    services.AddSingleton<INavigationViewService, NavigationViewService>();
                    services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                    services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
                    services.AddSingleton<IFileService, FileService>();

                    // Views and ViewModels
                    services.AddTransient<SettingsPage>();
                    services.AddTransient<SettingsViewModel>();
                    services.AddTransient<ShellPage>();
                    services.AddTransient<ShellViewModel>();

                    services.AddTransient<SchoolPage>();
                    services.AddTransient<SchoolViewModel>();
                    services.AddTransient<SubjectPage>();
                    services.AddTransient<SubjectViewModel>();
                }).Build();
        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TOODO : Handle unhandled exceptions
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}