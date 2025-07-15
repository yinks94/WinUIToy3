using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIToy3.Contracts.ViewModels;
using WinUIToy3.Services;
using WinUIToy3.Services.Models;

namespace WinUIToy3.ViewModels;

public partial class SectionViewModel : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private ObservableCollection<DataItem> _items;


    public void OnNavigatedFrom()
    {
        Debug.WriteLine("Navigated from SectionViewModel");
        if(_items != null)
        {
            _items.Clear();
            _items = null;
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        string uniqueId = parameter as string ?? string.Empty;
        if (!string.IsNullOrEmpty(uniqueId))
        {

            DataSource.GetGroupAsync(uniqueId).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var group = task.Result;
                    if (group != null)
                    {
                        _items = new ObservableCollection<DataItem>(group.Items);
                        Debug.WriteLine($"Loaded {_items.Count} items for group with uniqueId: {uniqueId}");
                    }
                    else
                    {
                        Debug.WriteLine("Group not found for uniqueId: " + uniqueId);
                    }
                }
                else
                {
                    Debug.WriteLine("Failed to load group for uniqueId: " + uniqueId);
                }
            });
            Debug.WriteLine("No uniqueId provided for SectionViewModel navigation.");
            return;
        }
        Debug.WriteLine("Navigated to SectionViewModel with parameter: " + parameter?.ToString());
    }
}
