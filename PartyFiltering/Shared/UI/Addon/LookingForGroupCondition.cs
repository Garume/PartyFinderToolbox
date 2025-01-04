using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Shared.UI.Addon;

public unsafe class LookingForGroupCondition(nint addon)
{
    private AtkUnitBase* Base { get; } = (AtkUnitBase*)addon;

    public AtkComponentButton* RecruitButton => Base->GetButtonNodeById(111);
    public AtkComponentButton* CancelButton => Base->GetButtonNodeById(112);
    public AtkComponentButton* ResetButton => Base->GetButtonNodeById(110);

    public AtkTextNode* CommentNode =>
        Base->GetComponentNodeById(22)->Component->UldManager.SearchNodeById(16)->GetAsAtkTextNode();

    public void Normal()
    {
        Base->GetButtonNodeById(3)->Click(Base);
    }

    public void Alliance()
    {
        Base->GetButtonNodeById(4)->Click(Base);
    }

    public void Recruit()
    {
        RecruitButton->Click(Base);
    }

    public void Cancel()
    {
        CancelButton->Click(Base);
    }

    public void Reset()
    {
        ResetButton->Click(Base);
    }

    public void SelectDutyCategory(byte i)
    {
        var evt = new AtkEvent();
        var data = AtkEventDataBuilder.Create().Write(16, i).Write(22, i).Build();
        Logger.Information(data.ToString(), true);
        Base->ReceiveEvent((AtkEventType)37, 7, &evt, &data);
    }
}