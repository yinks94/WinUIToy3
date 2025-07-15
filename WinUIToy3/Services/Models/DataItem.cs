using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIToy3.Services.Models;

public class DataItem : DataBaseInfo
{
    public object Parameter { get; set; }

    public bool IsHideItem { get; set; } = false;

    public override string ToString()
    {
        return $"{Title} ({UniqueId})";
    }
}
