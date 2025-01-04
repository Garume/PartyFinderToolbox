using PartyFinderToolbox.Core.Filters;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Core.UI;

public class TextFilterUI(TextFilter target) : FilterUI(target)
{
    public override void Draw()
    {
        var filter = (TextFilter)Target;
        filter.Text = ImGuiValue.InputText("##keyword", filter.Text, 30);
    }
}