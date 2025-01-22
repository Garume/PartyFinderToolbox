using PartyFinderToolbox.Core.Filters;

namespace PartyFinderToolbox.Core.Serializables;

public class Configuration
{
    [NonSerialized] public uint CurrentAutoRefreshLookingForGroupTime = 0;
    public List<Filter> Filters { get; set; } = [];
    public bool EnableAutoRefreshLookingForGroup { get; set; } = true;
    public uint AutoRefreshLookingForGroupInterval { get; set; } = 10;
    public bool EnableAutoReloadParty { get; set; } = true;
    public string AutoReloadPartyMessage { get; set; } = "";
    public Dictionary<string, RecruitmentSubDto> RecruitmentSubs { get; set; } = [];
}