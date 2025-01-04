using Dalamud.Game.Gui.PartyFinder.Types;

namespace PartyFinderToolbox.Core.Filters;

[Serializable]
public record TextFilter : Filter
{
    private string _text = string.Empty;

    public TextFilter() : this(string.Empty)
    {
    }

    public TextFilter(string text)
    {
        Text = text;
    }

    public string Text
    {
        get => _text;
        set => _text = value;
    }

    protected override bool IsContained(IPartyFinderListing listing)
    {
        return listing.Description.TextValue.Contains(Text);
    }
}