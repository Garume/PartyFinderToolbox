using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using PartyFinderToolbox.Shared.Services;

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

            PartyService.ApplyCondition(info);
        }
    }
}