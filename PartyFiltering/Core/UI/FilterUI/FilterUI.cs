using Dalamud.Interface;
using ImGuiNET;
using PartyFinderToolbox.Core.Filters;
using PartyFinderToolbox.Core.Serializables;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.UI;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Core.UI;

public abstract class FilterUI(Filter target, FilterUIOption option = FilterUIOption.None) : IUI
{
    protected readonly Filter Target = target;

    public abstract void Draw();

    internal static Filter InternalDraw(Filter filter)
    {
        return filter switch
        {
            OrFilter orFilter => new OrFilterUI(orFilter).InternalDraw(),
            AndFilter andFilter => new AndFilterUI(andFilter).InternalDraw(),
            TextFilter textFilter => new TextFilterUI(textFilter).InternalDraw(),
            PlayerFilter playerFilter => new PlayerFilterUI(playerFilter).InternalDraw(),
            CategoryFilter categoryFilter => new CategoryFilterUI(categoryFilter).InternalDraw(),
            _ => throw new NotImplementedException()
        };
    }

    private Filter InternalDraw()
    {
        if (!option.HasFlag(FilterUIOption.AlwaysEnabled))
        {
            Target.Enabled = ImGuiValue.Checkbox("##filteringkeyword-enabled", Target.Enabled);

            ImGui.SameLine();
        }

        if (!option.HasFlag(FilterUIOption.NoSelector))
        {
            ImGui.SetNextItemWidth(150f);
            if (ImGui.BeginCombo("##filteringkeyword-type", Target.GetType().Name))
            {
                foreach (var type in TypeCache.TryGetDerivedTypes<Filter>()!)
                    if (ImGui.Selectable(type.Name))
                    {
                        if (Activator.CreateInstance(type) is not Filter filter)
                        {
                            Logger.Warning($"Failed to cast {type.Name} to Filter.");
                            continue;
                        }

                        return filter;
                    }

                ImGui.EndCombo();
            }

            ImGui.SameLine();
        }


        ImGui.SetNextItemWidth(150f);
        Draw();

        if (!option.HasFlag(FilterUIOption.NoOption))
        {
            ImGui.SameLine();
            ImGui.SetNextItemWidth(150f);
            if (ImGui.BeginCombo("##filteringkeyword-option", Target.Option.ToString()))
            {
                ImGui.SetNextItemWidth(150f);
                if (ImGui.Selectable("Include")) Target.Option = FilterOption.Include;
                if (ImGui.Selectable("Exclude")) Target.Option = FilterOption.Exclude;
                ImGui.EndCombo();
            }
        }

        if (!option.HasFlag(FilterUIOption.NoDelete))
        {
            ImGui.SameLine();
            ImGui.PushFont(UiBuilder.IconFont);
            if (ImGui.Button($"{FontAwesomeIcon.Trash.ToIconString()}##filteringkeyword-remove"))
                Target.WillDelete = true;
            ImGui.PopFont();
        }

        return Target;
    }
}