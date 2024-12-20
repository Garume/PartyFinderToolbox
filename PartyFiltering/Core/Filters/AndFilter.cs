using Dalamud.Game.Gui.PartyFinder.Types;

namespace PartyFiltering.Core.Filters;

[Serializable]
public record AndFilter : Filter
{
    private List<Filter> _children = [];

    public List<Filter> Children
    {
        get => _children;
        set => _children = value;
    }


    protected override bool IsContained(IPartyFinderListing listing)
    {
        return Children.Count == 0 ||
               Children.All(x => !x.Enabled) ||
               Children.Where(x => x.Enabled).All(child => child.Apply(listing));
    }
}