using ImGuiNET;
using PartyFiltering.Filters;
using PartyFiltering.Serializables;
using PartyFiltering.Services;

namespace PartyFiltering.UI;

public class MainTab : Tab
{
    public override string Name => "Main";

    public override void Draw()
    {
        var config = ConfigService<Configuration>.Config;
        if (config is { Filters.Count: 0 }) config.Filters.Add(new OrFilter());

        for (var index = 0; index < config.Filters.Count; index++)
        {
            var filter = config.Filters[index];
            ImGui.PushID($"##filter-{filter.Id}");
            config.Filters[index] = FilterUI.InternalDraw(filter);
            if (filter.WillDelete) config.Filters.RemoveAt(index);
            ImGui.PopID();
        }

        if (ImGui.Button("Save")) ConfigService<Configuration>.Save("config.json");
    }
}