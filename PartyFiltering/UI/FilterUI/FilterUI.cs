using Dalamud.Interface;
using ImGuiNET;
using PartyFiltering.Filters;
using PartyFiltering.Serializables;
using PartyFiltering.Services;
using PartyFiltering.Utility;

namespace PartyFiltering.UI;

public abstract class FilterUI : IUI
{
    private readonly FilterUIOption _option;
    protected Filter Target;

    public FilterUI(Filter target)
    {
        Target = target;
    }

    public FilterUI(Filter target, FilterUIOption option)
    {
        Target = target;
        _option = option;
    }

    protected FilterUI()
    {
        throw new NotImplementedException();
    }

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
        if (!_option.HasFlag(FilterUIOption.AlwaysEnabled))
        {
            Target.Enabled = ImGuiValue.Checkbox("##filteringkeyword-enabled", Target.Enabled);

            ImGui.SameLine();
        }

        if (!_option.HasFlag(FilterUIOption.NoSelector))
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

        if (!_option.HasFlag(FilterUIOption.NoOption))
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

        if (!_option.HasFlag(FilterUIOption.NoDelete))
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