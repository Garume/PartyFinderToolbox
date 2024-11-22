using Dalamud.Plugin;
using PartyFiltering.Serializables;
using PartyFiltering.Services;

namespace PartyFiltering;

public class PartyFiltering : IDalamudPlugin
{
    private readonly ServiceContainer _serviceContainer;

    public PartyFiltering(IDalamudPluginInterface pluginInterface)
    {
        _serviceContainer = new ServiceContainer(
            new Logger(),
            new ConfigService<Configuration>(),
            new DataService(),
            new WindowService(),
            new PartyService());
        _serviceContainer.Init(pluginInterface);
        Logger.Debug("All services initialized");
    }

    public void Dispose()
    {
        _serviceContainer.Dispose();
        GC.SuppressFinalize(this);
    }
}