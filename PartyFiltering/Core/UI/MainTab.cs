using ImGuiNET;
using PartyFiltering.Core.Filters;
using PartyFiltering.Core.Serializables;
using PartyFiltering.Core.Services;
using PartyFiltering.Shared.Services;
using PartyFiltering.Shared.UI;

namespace PartyFiltering.Core.UI;

public class MainTab : Tab
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