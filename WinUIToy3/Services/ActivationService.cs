using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WinUIToy3.Contracts.Services;
using WinUIToy3.Services.Models;
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
        try
        {
            // Initialize the DataSource with the JSON file.
            await DataSource.Instance.GetGroupAsync(@"Assets\NavViewMenu\AppData.json");

#if DEBUG
            foreach (var group in DataSource.Instance.Groups)
            {
                foreach (var item in group.Items)
                {
                    Debug.WriteLine($"Menu(Group-Item): {group.Title} - {item.Title}" );
                }
               
            }
#endif
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during initialization: {ex.Message}");
        }


        //await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        //await _themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}