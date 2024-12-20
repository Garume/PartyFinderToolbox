using Dalamud.Game.Gui.PartyFinder.Types;

namespace PartyFiltering.Core.Filters;

public record PlayerFilter : Filter
{
    public PlayerFilter() : this(string.Empty)
    {
    }

    public PlayerFilter(string name)
    {
        PlayerName = name;
    }

    public string PlayerName { get; set; } = string.Empty;

    protected override bool IsContained(IPartyFinderListing listing)
    {
        return listing.Name.ToString() == PlayerName;
    }
}