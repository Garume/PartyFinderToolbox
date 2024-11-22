using ImGuiNET;
using PartyFiltering.Filters;

namespace PartyFiltering.UI;

public class AndFilterUI(AndFilter target) : FilterUI(target)
{
    public override void Draw()
    {
        ImGui.NewLine();
        var target = (AndFilter)Target;
        ImGui.Indent();
        for (var index = 0; index < target.Children.Count; index++)
        {
            var filter = target.Children[index];
            ImGui.PushID($"##filter-{filter.Id}");
            target.Children[index] = InternalDraw(filter);
            if (filter.WillDelete) target.Children.RemoveAt(index);
            ImGui.PopID();
        }

        if (ImGui.Button("Add")) target.Children.Add(new TextFilter());

        ImGui.Unindent();
    }
}