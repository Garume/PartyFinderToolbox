using ImGuiNET;
using PartyFinderToolbox.Core.Filters;
using PartyFinderToolbox.Core.Serializables;
using PartyFinderToolbox.Core.Services;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.UI;

namespace PartyFinderToolbox.Core.UI;

public class FilterTab : Tab
{
    public override string Name => "Main";

    public override void Draw()
    {
        var config = ConfigService<Configuration>.Config;
        if (config is { Filters.Count: 0 }) config.Filters.Add(new OrFilter());

        for (var index = 0; index < config?.Filters.Count; index++)
        {
            var filter = config.Filters[index];
            ImGui.PushID($"##filter-{filter.Id}");
            config.Filters[index] = FilterUI.InternalDraw(filter);
            if (filter.WillDelete) config.Filters.RemoveAt(index);
            ImGui.PopID();
        }

        if (ImGui.Button("Save")) ConfigurationConfigService.Save();

        ImGui.Separator();

        ImGui.Text(
            $"Refresh interval: {config.CurrentAutoRefreshLookingForGroupTime}/{config.AutoRefreshLookingForGroupInterval}");
    }
}