using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using ImGuiNET;
using PartyFinderToolbox.Core.Serializables;
using PartyFinderToolbox.Core.Services;
using PartyFinderToolbox.Shared.UI;

namespace PartyFinderToolbox.Core.UI;

public class PresetTab : Tab
{
    private DateTime _lastTimeStamp;
    private string _newPresetName = "";
    private string _selectedPreset = "";
    public override string Name => "Preset";

    public override unsafe void Draw()
    {
        var config = ConfigurationConfigService.Config;
        if (config == null) return;
        if (ImGui.BeginTable("presetTable", 2))
        {
            ImGui.TableSetupColumn("Preset");
            ImGui.TableSetupColumn("Detail");
            ImGui.TableHeadersRow();

            ImGui.TableNextColumn();
            var index = 0;
            foreach (var info in config.RecruitmentSubs)
            {
                ImGui.PushID($"recruitmentSubs{index}");
                if (ImGui.Selectable($"{info.Key}")) _selectedPreset = info.Key;

                if (ImGui.IsItemClicked(ImGuiMouseButton.Right)) ImGui.OpenPopup("LayoutContext");

                if (ImGui.BeginPopup("LayoutContext"))
                {
                    ImGui.InputText("New Name", ref _newPresetName, 100);
                    ImGui.SameLine();
                    if (ImGui.Button("Save"))
                        if (!string.IsNullOrEmpty(_newPresetName))
                        {
                            config.RecruitmentSubs.Add(_newPresetName, info.Value);
                            config.RecruitmentSubs.Remove(info.Key);
                            _selectedPreset = _newPresetName;
                        }

                    if (ImGui.Selectable("Delete"))
                    {
                        _selectedPreset = "";
                        config.RecruitmentSubs.Remove(info.Key);
                    }

                    ImGui.EndPopup();
                }

                ImGui.PopID();

                index++;
            }

            if (ImGui.Button("Add Current Setting"))
            {
                var duplicateCount = config.RecruitmentSubs.Count(x => x.Key.Contains("New Preset"));
                var current = AgentLookingForGroup.Instance()->StoredRecruitmentInfo;
                config.RecruitmentSubs.Add($"New Preset ({duplicateCount})", RecruitmentSubConverter.ToDto(current));
            }

            ImGui.TableNextColumn();

            if (!string.IsNullOrEmpty(_selectedPreset) &&
                config.RecruitmentSubs.TryGetValue(_selectedPreset, out var selectedPreset))
            {
                ImGui.Text($"DutyCategory:{selectedPreset.SelectedCategory}");
                ImGui.Text($"DutyID:{selectedPreset.SelectedDutyId}");
                ImGui.Text($"Objective:{selectedPreset.Objective}");
                ImGui.Text($"BeginnerFriendly:{selectedPreset.BeginnerFriendly}");
                ImGui.Text($"CompletionStatus:{selectedPreset.CompletionStatus}");
                ImGui.Text($"DutyFinderSettingFlags:{selectedPreset.DutyFinderSettingFlags}");
                ImGui.Text($"LootRule:{selectedPreset.LootRule}");
                ImGui.Text($"Password:{selectedPreset.Password}");
                ImGui.Text($"NumberOfSlotsInMainParty:{selectedPreset.NumberOfSlotsInMainParty}");
                ImGui.Text($"LimitRecruitingToWorld:{selectedPreset.LimitRecruitingToWorld}");
                ImGui.Text($"OnePlayerPerJob:{selectedPreset.OnePlayerPerJob}");
                ImGui.Text($"NumberOfGroups:{selectedPreset.NumberOfGroups}");

                for (var i = 0; i < selectedPreset.MemberContentIds.Length; i++)
                {
                    var contentId = selectedPreset.MemberContentIds[i];
                    if (contentId == 0) continue;
                    ImGui.Text($"MemberContentIds[{i}]:{contentId}");
                }

                for (var i = 0; i < selectedPreset.SlotFlags.Length; i++)
                {
                    var slotFlag = selectedPreset.SlotFlags[i];
                    if (slotFlag == 0) continue;
                    ImGui.Text($"MemberJobIds[{i}]:{selectedPreset.SlotFlags[i]}");
                }

                ImGui.Text($"Comment:{selectedPreset.CommentString}");


                if (ImGui.Button("Apply")) PartyService.ApplyCondition(selectedPreset);
                ImGui.SameLine();
                if (ImGui.Button("Apply and Start Party"))
                {
                    PartyService.ApplyCondition(selectedPreset);
                    PartyService.GetLookingForGroupCondition()?.Recruit();
                }
            }

            ImGui.EndTable();
        }
    }
}