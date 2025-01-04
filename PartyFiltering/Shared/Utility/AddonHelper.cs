using FFXIVClientStructs.FFXIV.Component.GUI;

namespace PartyFinderToolbox.Shared.Utility;

public static class AddonHelper
{
    public static unsafe void Click(this AtkComponentButton target, AtkUnitBase* addon)
    {
        if (!target.IsEnabled || !target.AtkResNode->IsVisible()) return;
        var btnRes = target.AtkComponentBase.OwnerNode->AtkResNode;
        var evt = btnRes.AtkEventManager.Event;

        addon->ReceiveEvent(evt->State.EventType, (int)evt->Param, btnRes.AtkEventManager.Event);
        var resetEvt = btnRes.AtkEventManager.Event;
        resetEvt->State.StateFlags = AtkEventStateFlags.None;
        addon->ReceiveEvent(resetEvt->State.EventType, (int)resetEvt->Param, btnRes.AtkEventManager.Event);
    }
}