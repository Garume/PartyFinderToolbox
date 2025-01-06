using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Shared.UI.Addon;

public unsafe class SelectYesNo(nint addon)
{
    private AtkUnitBase* Base { get; } = (AtkUnitBase*)addon;

    public AtkComponentButton* YesButton => Base->GetButtonNodeById(8);
    public AtkComponentButton* NoButton => Base->GetButtonNodeById(11);

    public void Yes()
    {
        YesButton->Click(Base);
    }

    public void No()
    {
        NoButton->Click(Base);
    }
}