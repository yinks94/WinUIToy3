using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIToy3.Services.Models;

public class DataGroup : DataBaseInfo
{
    

    public bool IsExpanded { get; set; } = false;

    public bool  IsHideGroup { get; set; }

    public ObservableCollection<DataItem> Items { get; set; } = new();   
}
