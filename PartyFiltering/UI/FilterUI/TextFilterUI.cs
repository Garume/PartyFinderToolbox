using PartyFiltering.Filters;

namespace PartyFiltering.UI;

[FilterUI(typeof(TextFilter))]
public class TextFilterUI(TextFilter target) : FilterUI(target)
{
    public override void Draw()
    {
        var filter = (TextFilter)Target;
        filter.Text = ImGuiValue.InputText("##keyword", filter.Text, 30);
    }
}