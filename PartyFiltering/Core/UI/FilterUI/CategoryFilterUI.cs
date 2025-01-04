using Dalamud.Game.Gui.PartyFinder.Types;
using ImGuiNET;
using Lumina.Excel.Sheets;
using PartyFinderToolbox.Core.Filters;
using PartyFinderToolbox.Core.Services;

namespace PartyFinderToolbox.Core.UI;

public class CategoryFilterUI(CategoryFilter target) : FilterUI(target)
{
    public override void Draw()
    {
        var filter = (CategoryFilter)Target;

        if (ImGui.BeginCombo("##categoryfilter-category", filter.Category.ToString()))
        {
            foreach (var category in Enum.GetNames(typeof(DutyCategory)))
                if (ImGui.Selectable(category))
                    filter.Category = Enum.Parse<DutyCategory>(category);
            ImGui.EndCombo();
        }

        if (filter.Category == DutyCategory.None)
            return;

        ImGui.SameLine();

        if (ImGui.BeginCombo("##categoryfilter-content",
                filter.ContentFinderCondition.HasValue
                    ? filter.ContentFinderCondition.Value.Name.ToString()
                    : "Select a content"))
        {
            foreach (var condition in DataService.Get<ContentFinderCondition>())
            {
                var contentName = condition.Name.ToString();

                switch (filter.Category)
                {
                    case DutyCategory.DutyRoulette when condition.LevelingRoulette:
                    case DutyCategory.HighEndDuty when condition.HighEndDuty:

                        if (ImGui.Selectable(contentName))
                            filter.ContentFinderConditionId = condition.RowId;
                        break;
                    case DutyCategory.Dungeon when condition.HighLevelRoulette:
                    case DutyCategory.Guildhest when condition.GuildHestRoulette:
                    case DutyCategory.Trial when condition.TrialRoulette:
                    case DutyCategory.Raid when condition.NormalRaidRoulette:
                    case DutyCategory.PvP when condition.PvP:
                    case DutyCategory.GoldSaucer when condition.MentorRoulette:
                    case DutyCategory.Fate when condition.FeastTeamRoulette:
                        if (ImGui.Selectable(contentName))
                            filter.ContentFinderConditionId = condition.RowId;
                        break;
                    case DutyCategory.None:
                    case DutyCategory.GatheringForay:
                    case DutyCategory.FieldOperation:
                    case DutyCategory.TreasureHunt:
                    case DutyCategory.TheHunt:
                    case DutyCategory.DeepDungeon:
                    case DutyCategory.VariantAndCriterionDungeon:
                    default:
                        break;
                }
            }

            ImGui.EndCombo();
        }
    }
}