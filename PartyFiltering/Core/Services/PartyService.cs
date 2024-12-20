using System.Diagnostics;
using Dalamud.Game.Gui.PartyFinder.Types;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFiltering.Core.Serializables;
using PartyFiltering.Shared.Services;
using PartyFiltering.Shared.Utility;

namespace PartyFiltering.Core.Services;

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
                //RefreshLookingForGroup();
                config.CurrentAutoRefreshLookingForGroupTime = 0;
                _lastTimeStamp = Stopwatch.GetTimestamp();
            }
        }
    }

    private unsafe void RefreshLookingForGroup()
    {
        var atk = (AtkUnitBase*)GameGui.GetAddonByName(WindowService.LookingForGroupAddonName);
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

    private void OnReceiveListing(IPartyFinderListing listing, IPartyFinderListingEventArgs args)
    {
        if (ConfigService<Configuration>.Config is not { } config) return;

        config.CurrentAutoRefreshLookingForGroupTime = 0;
        _lastTimeStamp = Stopwatch.GetTimestamp();

        args.Visible = config.Filters.Any(filter => filter.Apply(listing));
    }
}