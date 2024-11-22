using System.Diagnostics;
using Dalamud.Game.Gui.PartyFinder.Types;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using PartyFiltering.Serializables;
using PartyFiltering.Utility;

namespace PartyFiltering.Services;

public class PartyService : IIinitializable, IDisposable
{
    private readonly CompositeDisposable _disposables = new();

    private long _lastTimeStamp;
    [PluginService] private static IPartyFinderGui PartyFinder { get; set; } = null!;
    [PluginService] private static IGameGui GameGui { get; set; } = null!;
    [PluginService] private static IFramework Framework { get; set; } = null!;
    public static bool Initialized { get; private set; }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    public void Init(IDalamudPluginInterface pluginInterface)
    {
        if (Initialized) Logger.Debug("Services already initialized, skipping");
        Initialized = true;
        try
        {
            pluginInterface.Create<PartyService>();
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }

        PartyFinder.ReceiveListing += OnReceiveListing;
        _disposables.Add(() => PartyFinder.ReceiveListing -= OnReceiveListing);

        Framework.Update += Update;
        _disposables.Add(() => Framework.Update -= Update);
    }

    private void Update(IFramework framework)
    {
        var elapsed = Stopwatch.GetElapsedTime(_lastTimeStamp);
        if (ConfigService<Configuration>.Config is
            { EnableAutoRefreshLookingForGroup: true, AutoRefreshLookingForGroupInterval: > 0 } config)
        {
            config.CurrentAutoRefreshLookingForGroupTime += (uint)elapsed.Seconds;
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
        if (atk == null) return;
        try
        {
            var node = atk->UldManager.NodeList[5];
            node->GetAsAtkComponentButton()->IsChecked = true;
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