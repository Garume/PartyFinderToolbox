using Dalamud.Game.ClientState.Conditions;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using Lumina.Excel;
using PartyFinderToolbox.Shared.Services;

namespace PartyFinderToolbox.Core.Services;

[LoadService]
public class ConditionService : Service<ConditionService>
{
    [PluginService] private static ICondition Condition { get; set; } = null!;


    public static bool IsUsingPartyFinder()
    {
        return Condition[ConditionFlag.UsingPartyFinder];
    }
}