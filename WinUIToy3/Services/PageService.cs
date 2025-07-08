using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUIToy3.Contracts.Services;
using WinUIToy3.ViewModels;
using WinUIToy3.Views;

namespace WinUIToy3.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public Type GetPageType(string key)
    {
        Type? paeType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out paeType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }
        return paeType;
    }

    public PageService()
    {
        Configure<SettingsViewModel, SettingsPage>();
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }
            _pages.Add(key, type);
        }
    }
}