using Dalamud.Plugin;

namespace PartyFinderToolbox.Shared.Services;

public interface IIinitializable
{
    void Init(IDalamudPluginInterface pluginInterface);
}