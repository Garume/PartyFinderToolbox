using PartyFiltering.Filters;

namespace PartyFiltering.Serializables;

public class Configuration
{
    [NonSerialized] public uint CurrentAutoRefreshLookingForGroupTime = 0;
    public List<Filter> Filters { get; set; } = [];
    public bool EnableAutoRefreshLookingForGroup { get; set; } = true;
    public uint AutoRefreshLookingForGroupInterval { get; set; } = 10;
}