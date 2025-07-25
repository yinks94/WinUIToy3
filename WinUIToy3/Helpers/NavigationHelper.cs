﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUIToy3.Helpers;

public class NavigationHelper
{
    public static DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));

    public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

    public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);
}