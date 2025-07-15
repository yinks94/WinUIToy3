using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIToy3.Services.Models;

public class DataBaseInfo
{
    public string UniqueId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public string IconGlyph { get; set; } = string.Empty;

    public string SectionId { get; set; } = string.Empty;

    public string PageId { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = true;

    public bool UsexUid { get; set; }

}
