using System.Data;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Lumina.Excel;

namespace PartyFiltering.Services;

public class DataService : IIinitializable
{
    [PluginService] private static IDataManager DataManager { get; set; } = null!;
    
    public static bool Initialized { get; private set; }

    public void Init(IDalamudPluginInterface pluginInterface)
    {
        if (Initialized) Logger.Debug("Services already initialized, skipping");
        Initialized = true;
        try
        {
            pluginInterface.Create<DataService>();
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
    
    public static ExcelSheet<T> Get<T>() where T : struct,IExcelRow<T>
    {
        return DataManager.GetExcelSheet<T>(DataManager.Language);
    }
}