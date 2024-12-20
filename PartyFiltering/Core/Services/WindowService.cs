using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFiltering.Core.UI;
using PartyFiltering.Shared.Services;
using PartyFiltering.Shared.Utility;

namespace PartyFiltering.Core.Services;

[LoadService]
public class WindowService : Service<WindowService>
{
    public const string LookingForGroupAddonName = "LookingForGroup";
    private readonly ConfigWindow _configWindow = new();
    private readonly CompositeDisposable _disposables = [];
    [PluginService] private static IGameGui GameGui { get; set; } = null!;
    [PluginService] private static IFramework Framework { get; set; } = null!;

    protected override void Initialize()
    {
        var windowSystem = new WindowSystem();
        windowSystem.AddWindow(_configWindow);

        PluginInterface.UiBuilder.Draw += windowSystem.Draw;

        _disposables.Add(() => PluginInterface.UiBuilder.Draw -= windowSystem.Draw);

        Framework.Update += Update;
        _disposables.Add(() => Framework.Update -= Update);
    }

    private void Update(IFramework framework)
    {
        if (GameGui.GameUiHidden) return;
        if (_configWindow.IsOpen != DisplayLookingForGroupWindow())
        {
            ConfigurationConfigService.Save();
            _configWindow.Toggle();
        }
    }

    private unsafe bool DisplayLookingForGroupWindow()
    {
        var addon = (AtkUnitBase*)GameGui.GetAddonByName(LookingForGroupAddonName);
        return addon != null && addon->IsVisible;
    }
}