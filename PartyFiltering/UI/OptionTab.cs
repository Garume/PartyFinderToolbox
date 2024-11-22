using ImGuiNET;
using PartyFiltering.Serializables;
using PartyFiltering.Services;

namespace PartyFiltering.UI;

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
        config.EnableAutoRefreshLookingForGroup = ImGuiValue.Checkbox("Enable##EnableAutoRefreshLookingForGroup", config.EnableAutoRefreshLookingForGroup);
        config.AutoRefreshLookingForGroupInterval = ImGuiValue.InputUInt("Interval##AutoRefreshLookingForGroupInterval", config.AutoRefreshLookingForGroupInterval, 1, 60);
        ImGui.Unindent();
        ImGui.Separator();
    }
}