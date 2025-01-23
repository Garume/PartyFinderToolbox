using ImGuiNET;
using PartyFinderToolbox.Core.Serializables;
using PartyFinderToolbox.Core.Services;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.UI;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Core.UI;

public class OptionTab : Tab
{
    public override string Name => "Option";

    public override void Draw()
    {
        ImGui.Text("Option");
        var config = ConfigService<Configuration>.Config;
        if (config == null) return;

        ImGui.Text("Auto Refresh Party Finder");
        ImGui.Indent();
        config.EnableAutoRefreshLookingForGroup = ImGuiValue.Checkbox("Enable##EnableAutoRefreshLookingForGroup",
            config.EnableAutoRefreshLookingForGroup);
        config.AutoRefreshLookingForGroupInterval = ImGuiValue.InputUInt("Interval##AutoRefreshLookingForGroupInterval",
            config.AutoRefreshLookingForGroupInterval, 1, 60);
        ImGui.Unindent();
        ImGui.Separator();
        ImGui.Text("Reload Party");
        ImGui.Indent();
        config.EnableAutoReloadParty =
            ImGuiValue.Checkbox("Enable##EnableAutoReloadParty", config.EnableAutoReloadParty);
        config.AutoReloadPartyMessage =
            ImGuiValue.InputText("Message##AutoReloadPartyMessage", config.AutoReloadPartyMessage, 100);
        if (ImGui.Button("Reload Party")) PartyService.ReloadParty();
        ImGui.Unindent();
        ImGui.Separator();
    }
}