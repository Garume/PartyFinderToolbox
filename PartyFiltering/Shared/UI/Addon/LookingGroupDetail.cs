using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Shared.UI.Addon;

public unsafe class LookingForGroupDetail(nint addon)
{
    private AtkUnitBase* Base { get; } = (AtkUnitBase*)addon;

    public AtkComponentButton* ChangeButton => Base->GetButtonNodeById(109);
    public AtkComponentButton* CancelButton => Base->GetButtonNodeById(110);
    public AtkComponentButton* BackButton => Base->GetButtonNodeById(111);
    

    public void Change()
    {
        ChangeButton->Click(Base);
    }

    public void Cancel()
    {
        CancelButton->Click(Base);
    }

    public void Back()
    {
        BackButton->Click(Base);
    }
}