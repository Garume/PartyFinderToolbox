using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFiltering.Shared.Services;

namespace PartyFiltering.Shared.Utility;

public static class AddonHelper
{
    public static unsafe void Click(this AtkComponentButton target, AtkUnitBase* addon)
    {
        Logger.Warning($"Clicking button IsEnabled {target.IsEnabled} IsVisible {target.AtkResNode->IsVisible()}", true);
        
        if (!target.IsEnabled || !target.AtkResNode->IsVisible()) return;
        var btnRes = target.AtkComponentBase.OwnerNode->AtkResNode;
        var evt = btnRes.AtkEventManager.Event;
        
        Logger.Warning($"{btnRes} {evt->ToString()}", true);

        addon->ReceiveEvent(evt->State.EventType, (int)evt->Param, btnRes.AtkEventManager.Event);
        var resetEvt = btnRes.AtkEventManager.Event;
        resetEvt->State.StateFlags = AtkEventStateFlags.None;
        //addon->ReceiveEvent(resetEvt->State.EventType, (int)resetEvt->Param, btnRes.AtkEventManager.Event);
    }
}