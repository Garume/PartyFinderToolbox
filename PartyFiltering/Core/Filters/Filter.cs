using System.Text.Json.Serialization;
using Dalamud.Game.Gui.PartyFinder.Types;
using PartyFiltering.Core.Serializables;
using PartyFiltering.Shared.Services;

namespace PartyFiltering.Core.Filters;

[Serializable]
[JsonDerivedType(typeof(OrFilter), nameof(OrFilter))]
[JsonDerivedType(typeof(AndFilter), nameof(AndFilter))]
[JsonDerivedType(typeof(TextFilter), nameof(TextFilter))]
[JsonDerivedType(typeof(PlayerFilter), nameof(PlayerFilter))]
[JsonDerivedType(typeof(CategoryFilter), nameof(CategoryFilter))]
public abstract record Filter
{
    private bool _enabled = true;
    private Guid _id = Guid.NewGuid();
    private FilterOption _option = FilterOption.Include;

    protected Filter()
    {
    }

    public bool Enabled
    {
        get => AlwaysEnabled || _enabled;
        set => _enabled = value;
    }

    public FilterOption Option
    {
        get => _option;
        set => _option = value;
    }

    public Guid Id
    {
        get => _id;
        set => _id = value;
    }

    internal bool WillDelete { get; set; }
    protected virtual bool AlwaysEnabled => false;

    public bool Apply(IPartyFinderListing listing)
    {
        var isContained = IsContained(listing);
        return Option switch
        {
            FilterOption.Include => isContained,
            FilterOption.Exclude => !isContained,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected abstract bool IsContained(IPartyFinderListing listing);

    public static Filter? TryGetFromId(Guid id)
    {
        return ConfigService<Configuration>.Config?.Filters.FirstOrDefault(filter => filter.Id == id);
    }
}