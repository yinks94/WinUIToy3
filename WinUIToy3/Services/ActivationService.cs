using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using WinUIToy3.Contracts.Services;
using WinUIToy3.Views;

namespace WinUIToy3.Services;

public class ActivationService : IActivationService
{
    private UIElement? _shell = null;

    public ActivationService()
    {
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    private async Task InitializeAsync()
    {
        //await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        //await _themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}