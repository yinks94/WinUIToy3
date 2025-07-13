using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using WinUIToy3.Helpers;
using WinUIToy3.Services.Models;

namespace WinUIToy3.Services;


public class Root
{
    public ObservableCollection<DataGroup> Groups { get; set; }
}


[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(Root))]
internal partial class RootContext : JsonSerializerContext
{
}

public sealed class DataSource
{
    private static readonly object _lock = new();

    #region Singleton
    private static readonly DataSource _instance;

    public static DataSource Instance
    {
        get
        {
            return _instance;
        }
    }


    static DataSource()
    {
        _instance = new DataSource();
    }

    private DataSource()
    {
        _jsonFilePath = string.Empty;
        _pathType = PathType.Relative;
    }
    #endregion

    private string _jsonFilePath;

    private PathType _pathType;

    private readonly IList<DataGroup> _groups = new List<DataGroup>();

    public IList<DataGroup> Groups
    {
        get
        {
            return this._groups;

        }
    }


    public async Task<IEnumerable<DataGroup>> GetGroupAsync(string jsonFilePath, PathType pathType = PathType.Relative)
    {

        _jsonFilePath = jsonFilePath;
        _pathType = pathType;

        await _instance.GetControlInfoDataAsync();

        return _instance.Groups;
    }



    public static async Task<DataGroup> GetGroupAsync(string uniqueId)
    {
        await _instance.GetControlInfoDataAsync();
        var matches = _instance.Groups.Where(group => group.UniqueId.Equals(uniqueId));
        if( matches.Count() == 1 )
        {
            return matches.First();
        }
        return null;
    }

    public static async Task<DataItem> GetItemAsync(string uniqueId)
    {
        await _instance.GetControlInfoDataAsync();
        var matches = _instance.Groups.SelectMany(group => group.Items).Where(item => item.UniqueId == uniqueId);
        if( matches.Count() == 1 )
        {
            return matches.First();
        }
        return null;
    }


    public static async Task<DataGroup> GetGroupFromItemAsync(string uniqueId)
    {
        await _instance.GetControlInfoDataAsync();
        var matches = _instance.Groups.Where( (group) => group.Items.FirstOrDefault(item => item.UniqueId == uniqueId) != null );

        if( matches.Count() == 1 )
        {
            return matches.First();
        }
        return null;
    }


    private async Task GetControlInfoDataAsync()
    {
        lock(_lock)
        {
            if (this.Groups.Count > 0)
            {
                return; // Already loaded
            }
        }

        var jsonText = await LoadText(_jsonFilePath, _pathType);
        var controlInfoDataGroup = JsonSerializer.Deserialize(jsonText, typeof(Root), RootContext.Default) as Root;

        lock(_lock)
        {
#nullable enable

            //foreach (var group in controlInfoDataGroup.Groups)
            //{
            //    if (group.UsexUid)
            //    {
            //    }
            //}

            // TODO : item 세부 세팅루틴을 추가합니다.
            //controlInfoDataGroup
            //    .Groups.SelectMany(group => group.Items.Select(item => new { Group = group, Item = item }))
            //    .ToList()
            //    .ForEach(x =>
            //    {
            //        var group = x.Group;
            //        var item = x.Item;


            //    });

            foreach (var group in controlInfoDataGroup.Groups)
            {
                if( !Groups.Any(g => g.Title == group.Title) )
                {
                    Groups.Add(group);
                }
            }
#nullable disable
        }

    }


    public static async Task<string> LoadText(String filePath, PathType pathType)
    {
        StorageFile file = null;

        if (!PackageHelper.IsPackaged)
        {
            if(pathType == PathType.Relative)
            {
                var sourcePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(ProcessInfoHelper.GetFileVersionInfo().FileName), filePath));
                file = await StorageFile.GetFileFromPathAsync(sourcePath);
            }
            else if (pathType == PathType.Absolute)
            {
                file = await StorageFile.GetFileFromPathAsync(filePath);
            }
        }
        else
        {
            if(pathType == PathType.Relative)
            {
                Uri sourceUri = new Uri("ms-appx:///" + filePath);
                file = await StorageFile.GetFileFromApplicationUriAsync(sourceUri);

            }
            else if (pathType == PathType.Absolute)
            {
                Uri sourceUri = new Uri(filePath);
                file = await StorageFile.GetFileFromApplicationUriAsync(sourceUri);
            }
        }
        return await FileIO.ReadTextAsync(file);

    }
}
