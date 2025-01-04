using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using ImGuiNET;
using PartyFinderToolbox.Shared.UI;

namespace PartyFinderToolbox.Core.UI;

public class DebugTab : Tab
{
    public override string Name => "Debug";

    public override unsafe void Draw()
    {
        ImGui.Text("Debug");
        var storedInfo = AgentLookingForGroup.Instance()->StoredRecruitmentInfo;
        ImGui.Text($"DutyCategory:{storedInfo.SelectedCategory}");
        ImGui.Text($"DutyID:{storedInfo.SelectedDutyId}");
        ImGui.Text($"Objective:{storedInfo.Objective}");
        ImGui.Text($"BeginnerFriendly:{storedInfo.BeginnerFriendly}");
        ImGui.Text($"CompletionStatus:{storedInfo.CompletionStatus}");
        ImGui.Text($"DutyFinderSettingFlags:{storedInfo.DutyFinderSettingFlags}");
        ImGui.Text($"LootRule:{storedInfo.LootRule}");
        ImGui.Text($"Password:{storedInfo.Password}");
        ImGui.Text($"NumberOfSlotsInMainParty:{storedInfo.NumberOfSlotsInMainParty}");
        ImGui.Text($"LimitRecruitingToWorld:{storedInfo.LimitRecruitingToWorld}");
        ImGui.Text($"OnePlayerPerJob:{storedInfo.OnePlayerPerJob}");
        ImGui.Text($"NumberOfGroups:{storedInfo.NumberOfGroups}");

        for (var i = 0; i < storedInfo.MemberContentIds.Length; i++)
            ImGui.Text($"MemberContentIds[{i}]:{storedInfo.MemberContentIds[i]}");

        for (var i = 0; i < storedInfo.SlotFlags.Length; i++)
            ImGui.Text($"MemberJobIds[{i}]:{storedInfo.SlotFlags[i]}");

        ImGui.Text($"Comment:{storedInfo.CommentString}");
    }
}