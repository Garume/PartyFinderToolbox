using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFiltering.Serializables;
using PartyFiltering.UI;
using PartyFiltering.Utility;

namespace PartyFiltering.Services;

public class WindowService : IIinitializable, IDisposable
{
    public const string LookingForGroupAddonName = "LookingForGroup";
    private readonly CompositeDisposable _disposables = [];
    private readonly ConfigWindow _configWindow = new();
    [PluginService] private static IGameGui GameGui { get; set; } = null!;
    [PluginService] private static IFramework Framework { get; set; } = null!;
    public static bool Initialized { get; private set; }


    public void Dispose()
    {
        _disposables.Dispose();
    }

    public void Init(IDalamudPluginInterface pluginInterface)
    {
        if (Initialized) Logger.Debug("Services already initialized, skipping");
        Initialized = true;
        try
        {
            pluginInterface.Create<WindowService>();
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }

        var windowSystem = new WindowSystem();
        windowSystem.AddWindow(_configWindow);

        pluginInterface.UiBuilder.Draw += windowSystem.Draw;

        _disposables.Add(() => pluginInterface.UiBuilder.Draw -= windowSystem.Draw);

        Framework.Update += Update;
        _disposables.Add(() => Framework.Update -= Update);
    }

    private void Update(IFramework framework)
    {
        if (GameGui.GameUiHidden) return;
        if (_configWindow.IsOpen != DisplayLookingForGroupWindow())
        {
            ConfigService<Configuration>.Save("config.json");
            _configWindow.Toggle();
        }

        ;
    }

    private unsafe bool DisplayLookingForGroupWindow()
    {
        var addon = (AtkUnitBase*)GameGui.GetAddonByName(LookingForGroupAddonName);
        return addon != null && addon->IsVisible;
    }
}