using System.Collections;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFinderToolbox.Core.Serializables;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.Services.Task.YieldInstruction;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Core.Services;

[LoadService]
public class CommandService : Service<CommandService>
{
    [PluginService] private static ICommandManager Command { get; set; } = null!;

    protected override void Initialize()
    {
        Command.AddHandler("/pft", new CommandInfo(OnCommand));
    }

    private void OnCommand(string command, string args)
    {
        var splitArgs = args.Split(" ");
        if (splitArgs[0] == "test") Logger.Information("Test command executed!", true);
        if (splitArgs[0] == "create")
        {
            var presetName = splitArgs[1];
            var info = ConfigurationConfigService.Config.RecruitmentSubs[presetName];
            Logger.Information($"Preset {presetName} created with comment: {info.CommentString}", true);

            TaskService.StartCoroutine(ApplyCondition(info));
        }
    }

    public IEnumerator ApplyCondition(RecruitmentSubDto info)
    {
        if (!DisplayLookingForGroupWindow()) ChatService.ExecuteCommand("/pfinder");
        yield return new WaitUntil(DisplayLookingForGroupWindow);
        ClickCondition();
        yield return new WaitUntil(DisplayLookingForGroupConditionWindow);
        RecruitmentSubConverter.Apply(info);
        yield return new WaitForSeconds(1);
        
        ChatService.ExecuteCommand("/pfinder");
        yield return new WaitWile(DisplayLookingForGroupWindow);
        
        ChatService.ExecuteCommand("/pfinder");
        yield return new WaitUntil(DisplayLookingForGroupWindow);
        ClickCondition();

        yield return null;
    }

    public unsafe bool DisplayLookingForGroupWindow()
    {
        var addon = PartyService.GetLookingForGroupAddon();
        return addon != null && addon->IsReady();
    }
    
    public unsafe bool DisplayLookingForGroupConditionWindow()
    {
        var addon = PartyService.GetLookingForGroupConditionAddon();
        return addon != null && addon->IsReady();
    }

    public unsafe void ClickCondition()
    {
        PartyService.GetLookingForGroupAddon()->GetComponentNodeById(46)->GetAsAtkComponentButton()->Click(
            PartyService.GetLookingForGroupAddon());
    }
}