using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WinUIToy3.Contracts.Services;
using WinUIToy3.Helpers;
using WinUIToy3.ViewModels;

namespace WinUIToy3.Services;

public class NavigationViewService : INavigationViewService
{
    private readonly INavigationService _navigationService;

    private readonly IPageService _pageService;

    private NavigationView? _navigationView;

    public NavigationViewService(INavigationService navigationService, IPageService pageService)
    {
        _navigationService = navigationService;
        _pageService = pageService;
    }

    public IList<object>? MenuItems => _navigationView?.MenuItems;

    public object? SettingsItem => _navigationView?.SettingsItem;

    public NavigationViewItem? GetSelectedItem(Type pageType)
    {
        if (_navigationView != null)
        {
            return GetSelectedItem(_navigationView.MenuItems, pageType) ?? GetSelectedItem(_navigationView.FooterMenuItems, pageType);
        }

        return null;
    }

    [MemberNotNull(nameof(_navigationView))]
    public void Initialize(NavigationView navigationView)
    {
        InitializeMenuItems(navigationView);

    }

    private void InitializeMenuItems(NavigationView navigationView)
    {
        Reset();
        _navigationView = navigationView ?? throw new ArgumentNullException(nameof(navigationView));
        AddNavigationMenuItems();
        _navigationView.BackRequested += OnBackRequested;
        _navigationView.ItemInvoked += OnItemInvoked;
    }



    private void Reset()
    {
        if( _navigationView != null)
        {
            _navigationView.MenuItems?.Clear();
            _navigationView.FooterMenuItems.Clear();
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }

    }

    private void AddNavigationMenuItems()
    {
        foreach (var group in DataSource.Instance.Groups.Where(i => !i.IsHideGroup))
        {
            var itemGroup = new NavigationViewItem()
            {
                Content = group.Title,
                IsExpanded = group.IsExpanded,
                Tag = group.UniqueId,
                DataContext = group
            };

            var groupIcon = GetIcon(group.ImagePath, group.IconGlyph);
            if (groupIcon != null)
            {
                itemGroup.Icon = groupIcon;
            }

            foreach (var item in group.Items.Where(i => !i.IsHideItem))
            {
                var itemInGroup = new NavigationViewItem()
                {
                    Content = item.Title,
                    Tag = item.UniqueId,
                    DataContext = item,
                };

                var itemIcon = GetIcon(item.ImagePath, item.IconGlyph);
                if (itemIcon != null)
                {
                    itemInGroup.Icon = itemIcon;
                }
                itemGroup.MenuItems.Add(itemInGroup);
            }
            _navigationView?.MenuItems.Add(itemGroup);
        }
    }

    private IconElement GetIcon(string imagePath, string iconGlyph)
    {
        if( string.IsNullOrEmpty(imagePath) && string.IsNullOrEmpty(iconGlyph))
        {
            return null;
        }

        if( !string.IsNullOrEmpty(iconGlyph))
        {
            return GetFontIcon(iconGlyph);
        }

        if( !string.IsNullOrEmpty(imagePath))
        {
            return new BitmapIcon()
            {
                UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute),
                ShowAsMonochrome = false,
            };
        }
        return null;
    }

    private FontIcon GetFontIcon(string glyph)
    {
        var fontIcon = new FontIcon();
        if( !string.IsNullOrEmpty(glyph))
        {
            fontIcon.Glyph = glyph;
        }
        return fontIcon;

    }

    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        }
        else
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
            {
                var pageType = _pageService.GetPageType(pageKey);
                if (pageType != null)
                {
                    _navigationService.NavigateTo(pageType.FullName!);
                }
                else
                {
                    Debug.WriteLine($"Page type for key '{pageKey}' not found.");
                }
            }
            else
            {
                Debug.WriteLine("Selected item does not have a valid NavigateTo property.");
            }
        }
    }

    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        _navigationService.GoBack();
    }

    public void UnregisterEvents()
    {
        if (_navigationView != null)
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
            //_navigationView = null;
        }
    }

    private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (IsMenuItemForPageType(item, pageType))
            {
                return item;
            }

            var selectedChild = GetSelectedItem(item.MenuItems, pageType);
            if (selectedChild != null)
            {
                return selectedChild;
            }
        }

        return null;
    }

    private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
    {
        if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
        {
            return _pageService.GetPageType(pageKey) == sourcePageType;
        }

        return false;
    }
}