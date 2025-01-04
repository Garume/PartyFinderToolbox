using System.Reflection;
using Dalamud.Plugin;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Shared;

public class Plugin : IDalamudPlugin
{
    private readonly ServiceContainer _serviceContainer = new();

    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        var service = TypeCache.TryGetTypesWithAttribute<LoadServiceAttribute>()?
            .Where(x => !x.GetCustomAttribute<LoadServiceAttribute>()?.Disable ?? true)
            .OrderByDescending(x => x.GetCustomAttribute<LoadServiceAttribute>()?.Priority)
            .Select(x => (IIinitializable)Activator.CreateInstance(x)!)
            .ToArray();

        if (service != null) _serviceContainer = new ServiceContainer(service);
        _serviceContainer.Init(pluginInterface);
        Logger.Debug("All services initialized");
    }

    public void Dispose()
    {
        _serviceContainer.Dispose();
        GC.SuppressFinalize(this);
    }
}