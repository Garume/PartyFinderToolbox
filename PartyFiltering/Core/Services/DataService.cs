using Dalamud.IoC;
using Dalamud.Plugin.Services;
using Lumina.Excel;
using PartyFinderToolbox.Shared.Services;

namespace PartyFinderToolbox.Core.Services;

[LoadService]
public class DataService : Service<DataService>
{
    [PluginService] private static IDataManager DataManager { get; set; } = null!;


    public static ExcelSheet<T> Get<T>() where T : struct, IExcelRow<T>
    {
        return DataManager.GetExcelSheet<T>(DataManager.Language);
    }
}