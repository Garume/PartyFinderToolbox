using Dalamud.Plugin;

namespace PartyFiltering.Shared.Services;

public class ServiceContainer(params IIinitializable[] services) : IDisposable, IIinitializable
{
    public void Dispose()
    {
        foreach (var service in services) (service as IDisposable)?.Dispose();
        GC.SuppressFinalize(this);
    }

    public void Init(IDalamudPluginInterface pluginInterface)
    {
        foreach (var service in services) service.Init(pluginInterface);
    }
}