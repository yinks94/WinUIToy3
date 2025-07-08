using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;
using WinUIToy3.Contracts.Services;
using WinUIToy3.Views;

namespace WinUIToy3.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    public INavigationService NavigationService
    {
        get;
        private set;
    }

    public INavigationViewService NavigationViewService
    {
        get;
        private set;
    }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        // Initialization logic can go here if needed
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
        else
        {
            // If no item is found, you can set a default or handle it accordingly
            Selected = NavigationViewService.MenuItems?.FirstOrDefault();
        }
    }
}