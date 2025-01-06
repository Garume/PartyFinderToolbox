using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFinderToolbox.Core.UI;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Core.Services;

[LoadService]
public class WindowService : Service<WindowService>
{
    public const string LookingForGroupAddonName = "LookingForGroup";
    public const string LookingForGroupConditionAddonName = "LookingForGroupCondition";
    public const string LookingForGroupDetailAddonName = "LookingForGroupDetail";
    public const string SelectYesNoAddonName = "SelectYesNo";
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