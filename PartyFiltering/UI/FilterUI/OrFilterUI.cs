using ImGuiNET;
using PartyFiltering.Filters;

namespace PartyFiltering.UI;

public class OrFilterUI(OrFilter target) : FilterUI(target,
    FilterUIOption.AlwaysEnabled | FilterUIOption.NoSelector | FilterUIOption.NoOption | FilterUIOption.NoDelete)
{
    public override void Draw()
    {
        var target = (OrFilter)Target;
        for (var index = 0; index < target.Children.Count; index++)
        {
            var filter = target.Children[index];
            ImGui.PushID($"##filter-{filter.Id}");
            target.Children[index] = InternalDraw(filter);
            if (filter.WillDelete) target.Children.RemoveAt(index);
            ImGui.PopID();
        }

        if (ImGui.Button("Add")) target.Children.Add(new TextFilter());
    }
}