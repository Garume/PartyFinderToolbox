using System.Text.Json.Serialization;
using Dalamud.Game.Gui.PartyFinder.Types;
using Lumina.Excel.Sheets;
using PartyFinderToolbox.Core.Services;

namespace PartyFinderToolbox.Core.Filters;

[Serializable]
public record CategoryFilter : Filter
{
    private DutyCategory _category = DutyCategory.None;
    private uint _contentFinderConditionId;

    public CategoryFilter() : this(DutyCategory.None)
    {
    }

    public CategoryFilter(DutyCategory category)
    {
        Category = category;
    }

    public DutyCategory Category
    {
        get => _category;
        set => _category = value;
    }

    [JsonIgnore]
    public ContentFinderCondition? ContentFinderCondition =>
        DataService.Get<ContentFinderCondition>().GetRowOrDefault(_contentFinderConditionId);

    public uint ContentFinderConditionId
    {
        get => _contentFinderConditionId;
        set => _contentFinderConditionId = value;
    }

    protected override bool IsContained(IPartyFinderListing listing)
    {
        var isContained = listing.Category == _category;
        if (_category != DutyCategory.None)
            isContained = listing.Duty.ValueNullable?.TerritoryType.ValueNullable?.ContentFinderCondition.RowId ==
                          ContentFinderConditionId;

        return isContained;
    }
}