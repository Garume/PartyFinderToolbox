using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace PartyFiltering.UI;

public class ConfigWindow : Window
{
    public ConfigWindow(ImGuiWindowFlags flags = ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse,
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
            var tabs = new Tab[]
            {
                new MainTab(),
                new OptionTab()
            };

            foreach (var tab in tabs)
                if (ImGui.BeginTabItem(tab.Name))
                {
                    tab.Draw();
                    ImGui.EndTabItem();
                }

            ImGui.EndTabBar();
        }
    }
}