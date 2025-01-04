using Dalamud.Interface.Windowing;
using ImGuiNET;
using PartyFinderToolbox.Shared.UI;

namespace PartyFinderToolbox.Core.UI;

public class ConfigWindow : Window
{
    private static IEnumerable<Tab> _tabs = new Tab[]
    {
        new PresetTab(),
        new FilterTab(),
        new OptionTab(),
#if DEBUG
        new DebugTab()
#endif
    };

    public ConfigWindow(ImGuiWindowFlags flags = ImGuiWindowFlags.AlwaysAutoResize,
        bool forceMainWindow = false) : base(
        "PartyFiltering", flags, forceMainWindow)
    {
        ShowCloseButton = false;
    }

    public override void Draw()
    {
        if (!IsOpen) return;

        if (ImGui.BeginTabBar("PartyFilteringTabs"))
        {
            foreach (var tab in _tabs)
                if (ImGui.BeginTabItem(tab.Name))
                {
                    tab.Draw();
                    ImGui.EndTabItem();
                }

            ImGui.EndTabBar();
        }
    }
}