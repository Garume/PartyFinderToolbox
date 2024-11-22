using Dalamud.Game.Gui.PartyFinder.Types;
using PartyFiltering.Services;

namespace PartyFiltering.Filters;

[Serializable]
public record OrFilter : Filter
{
    private List<Filter> _children = [];

    public List<Filter> Children
    {
        get => _children;
        set => _children = value;
    }

    protected override bool AlwaysEnabled => true;

    protected override bool IsContained(IPartyFinderListing listing)
    {
        Logger.Error($"PartyName:{listing.Name}", true);

        return Children.Count == 0 ||
               Children.All(x => !x.Enabled) ||
               Children.Where(x => x.Enabled).Any(child => child.Apply(listing));
    }
}