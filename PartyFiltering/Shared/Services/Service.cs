using Dalamud.Plugin;
using PartyFinderToolbox.Shared.Utility;

namespace PartyFinderToolbox.Shared.Services;

public abstract class Service<T> : IIinitializable, IDisposable where T : class
{
    protected readonly CompositeDisposable Disposables = new();
    protected static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    protected static bool Initialized { get; private set; }

    public void Dispose()
    {
        Disposables.Dispose();
    }

    public void Init(IDalamudPluginInterface pluginInterface)
    {
        if (Initialized) Logger.Debug("Services already initialized, skipping");
        Initialized = true;
        try
        {
            pluginInterface.Create<T>();
        }
        catch (Exception ex)
        {
            Logger.Error($"{ex.Message} \n {ex.StackTrace}");
        }

        PluginInterface = pluginInterface;
        Initialize();
    }

    protected virtual void Initialize()
    {
    }
}