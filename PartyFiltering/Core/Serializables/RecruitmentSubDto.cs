using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace PartyFinderToolbox.Core.Serializables;

public class RecruitmentSubDto
{
    public ushort SelectedCategory { get; set; }
    public ushort SelectedDutyId { get; set; }
    public byte BeginnerFriendly { get; set; }
    public AgentLookingForGroup.CompletionStatus CompletionStatus { get; set; }
    public AgentLookingForGroup.Objective Objective { get; set; }
    public AgentLookingForGroup.DutyFinderSetting DutyFinderSettingFlags { get; set; }
    public AgentLookingForGroup.LootRule LootRule { get; set; }
    public ushort Password { get; set; }
    public AgentLookingForGroup.Language LanguageFlags { get; set; }
    public byte NumberOfSlotsInMainParty { get; set; }
    public byte LimitRecruitingToWorld { get; set; }
    public byte OnePlayerPerJob { get; set; }
    public byte NumberOfGroups { get; set; }

    public ulong[] MemberContentIds { get; set; } = Array.Empty<ulong>();

    public ulong[] SlotFlags { get; set; } = Array.Empty<ulong>();

    public string CommentString { get; set; } = new(' ', 0x100 - 1);
}

public static class RecruitmentSubConverter
{
    public static RecruitmentSubDto ToDto(AgentLookingForGroup.RecruitmentSub sub)
    {
        return new RecruitmentSubDto
        {
            SelectedCategory = sub.SelectedCategory,
            SelectedDutyId = sub.SelectedDutyId,
            BeginnerFriendly = sub.BeginnerFriendly,
            CompletionStatus = sub.CompletionStatus,
            Objective = sub.Objective,
            DutyFinderSettingFlags = sub.DutyFinderSettingFlags,
            LootRule = sub.LootRule,
            Password = sub.Password,
            LanguageFlags = sub.LanguageFlags,
            NumberOfSlotsInMainParty = sub.NumberOfSlotsInMainParty,
            LimitRecruitingToWorld = sub.LimitRecruitingToWorld,
            OnePlayerPerJob = sub.OnePlayerPerJob,
            NumberOfGroups = sub.NumberOfGroups,

            MemberContentIds = sub.MemberContentIds.ToArray(),
            SlotFlags = sub.SlotFlags.ToArray(),
            CommentString = sub.CommentString
        };
    }

    public static unsafe void Apply(RecruitmentSubDto dto)
    {
        var info = &AgentLookingForGroup.Instance()->StoredRecruitmentInfo;

        info->SelectedCategory = dto.SelectedCategory;
        info->SelectedDutyId = dto.SelectedDutyId;
        info->BeginnerFriendly = dto.BeginnerFriendly;
        info->CompletionStatus = dto.CompletionStatus;
        info->Objective = dto.Objective;
        info->DutyFinderSettingFlags = dto.DutyFinderSettingFlags;
        info->LootRule = dto.LootRule;
        info->Password = dto.Password;
        info->LanguageFlags = dto.LanguageFlags;
        info->NumberOfSlotsInMainParty = dto.NumberOfSlotsInMainParty;
        info->LimitRecruitingToWorld = dto.LimitRecruitingToWorld;
        info->OnePlayerPerJob = dto.OnePlayerPerJob;
        info->NumberOfGroups = dto.NumberOfGroups;

        for (var i = 0; i < dto.MemberContentIds.Length; i++) info->MemberContentIds[i] = dto.MemberContentIds[i];

        for (var i = 0; i < dto.SlotFlags.Length; i++) info->SlotFlags[i] = dto.SlotFlags[i];

        info->CommentString = new string(' ', 192 - 1);
        info->CommentString = dto.CommentString;
    }
}