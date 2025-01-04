using PartyFinderToolbox.Core.Filters;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Core.UI;

public class PlayerFilterUI(PlayerFilter target) : FilterUI(target)
{
    public override void Draw()
    {
        var filter = (PlayerFilter)Target;
        filter.PlayerName = ImGuiValue.InputText("##keyword", filter.PlayerName, 30);
    }
}