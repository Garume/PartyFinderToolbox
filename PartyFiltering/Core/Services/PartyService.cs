using System.Collections;
using System.Diagnostics;
using Dalamud.Game.Gui.PartyFinder.Types;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFinderToolbox.Core.Serializables;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.Services.Task;
using PartyFinderToolbox.Shared.Services.Task.YieldInstruction;
using PartyFinderToolbox.Shared.UI.Addon;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Core.Services;

[LoadService]
public class PartyService : Service<PartyService>
{
    private long _lastTimeStamp;
    [PluginService] private static IPartyFinderGui PartyFinder { get; set; } = null!;
    [PluginService] private static IGameGui GameGui { get; set; } = null!;
    [PluginService] private static IFramework Framework { get; set; } = null!;


    protected override void Initialize()
    {
        PartyFinder.ReceiveListing += OnReceiveListing;
        Disposables.Add(() => PartyFinder.ReceiveListing -= OnReceiveListing);

        Framework.Update += Update;
        Disposables.Add(() => Framework.Update -= Update);
    }

    private void Update(IFramework framework)
    {
        if (ConfigService<Configuration>.Config is
            { EnableAutoRefreshLookingForGroup: true, AutoRefreshLookingForGroupInterval: > 0 } config)
        {
            var elapsed = Stopwatch.GetElapsedTime(_lastTimeStamp);
            var elapsedSeconds = elapsed.Seconds;

            if (elapsedSeconds > config.CurrentAutoRefreshLookingForGroupTime)
                config.CurrentAutoRefreshLookingForGroupTime += 1;

            if (config.CurrentAutoRefreshLookingForGroupTime >= config.AutoRefreshLookingForGroupInterval)
            {
                RefreshLookingForGroup();
                config.CurrentAutoRefreshLookingForGroupTime = 0;
                _lastTimeStamp = Stopwatch.GetTimestamp();
            }
        }
    }

    private unsafe void RefreshLookingForGroup()
    {
        var atk = (AtkUnitBase*)GameGui.GetAddonByName(WindowService.LookingForGroupAddonName);
        if (atk == null || !atk->IsVisible) return;
        try
        {
            var node = atk->GetButtonNodeById(47);
            node->Click(atk);
        }
        catch (Exception e)
        {
            Logger.Error(e.Message, true);
        }
    }

    public static void ApplyCondition(RecruitmentSubDto info)
    {
        TaskService.StartCoroutine(ApplyConditionCoroutine(info));
    }

    public static void ApplyConditionAndStart(RecruitmentSubDto info)
    {
        TaskService.StartCoroutine(ApplyConditionAndStartCoroutine(info));
    }

    private static IEnumerator ApplyConditionCoroutine(RecruitmentSubDto info)
    {
        if (!DisplayLookingForGroupWindow()) ChatService.ExecuteCommand("/pfinder");
        yield return new WaitUntil(DisplayLookingForGroupWindow);
        ClickCondition();
        yield return new WaitUntil(DisplayLookingForGroupConditionWindow);
        var condition = GetLookingForGroupCondition();
        if (info.NumberOfGroups == 1)
            condition?.Normal();
        else
            condition?.Alliance();
        RecruitmentSubConverter.Apply(info);
        yield return new WaitForSeconds(1);

        ChatService.ExecuteCommand("/pfinder");
        yield return new WaitWile(DisplayLookingForGroupWindow);

        ChatService.ExecuteCommand("/pfinder");
        yield return new WaitUntil(DisplayLookingForGroupWindow);
        ClickCondition();

        yield return null;
    }

    private static IEnumerator ApplyConditionAndStartCoroutine(RecruitmentSubDto info)
    {
        if (!DisplayLookingForGroupWindow()) ChatService.ExecuteCommand("/pfinder");
        yield return new WaitUntil(DisplayLookingForGroupWindow);
        ClickCondition();
        yield return new WaitUntil(DisplayLookingForGroupConditionWindow);
        var condition = GetLookingForGroupCondition();
        if (info.NumberOfGroups == 1)
            condition?.Normal();
        else
            condition?.Alliance();
        RecruitmentSubConverter.Apply(info);
        yield return new WaitForSeconds(1);

        ChatService.ExecuteCommand("/pfinder");
        yield return new WaitWile(DisplayLookingForGroupWindow);

        ChatService.ExecuteCommand("/pfinder");
        yield return new WaitUntil(DisplayLookingForGroupWindow);
        ClickCondition();
        yield return new WaitUntil(DisplayLookingForGroupConditionWindow);
        GetLookingForGroupCondition()?.Recruit();
        yield return null;
    }

    private static unsafe bool DisplayLookingForGroupWindow()
    {
        var addon = GetLookingForGroupAddon();
        return addon != null && addon->IsReady();
    }

    private static unsafe bool DisplayLookingForGroupConditionWindow()
    {
        var addon = GetLookingForGroupConditionAddon();
        return addon != null && addon->IsReady();
    }

    private static unsafe void ClickCondition()
    {
        GetLookingForGroupAddon()->GetComponentNodeById(46)->GetAsAtkComponentButton()->Click(
            GetLookingForGroupAddon());
    }

    private static unsafe LookingForGroupCondition? GetLookingForGroupCondition()
    {
        var addon = GetLookingForGroupConditionAddon();
        return addon == null ? null : new LookingForGroupCondition((IntPtr)addon);
    }


    private static unsafe AtkUnitBase* GetLookingForGroupConditionAddon()
    {
        return (AtkUnitBase*)GameGui.GetAddonByName(WindowService.LookingForGroupConditionAddonName);
    }

    private static unsafe AtkUnitBase* GetLookingForGroupAddon()
    {
        return (AtkUnitBase*)GameGui.GetAddonByName(WindowService.LookingForGroupAddonName);
    }


    private void OnReceiveListing(IPartyFinderListing listing, IPartyFinderListingEventArgs args)
    {
        if (ConfigService<Configuration>.Config is not { } config) return;

        config.CurrentAutoRefreshLookingForGroupTime = 0;
        _lastTimeStamp = Stopwatch.GetTimestamp();

        args.Visible = config.Filters.Any(filter => filter.Apply(listing));
    }
}