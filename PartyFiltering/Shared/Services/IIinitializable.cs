using Dalamud.Plugin;

namespace PartyFiltering.Shared.Services;

public interface IIinitializable
{
    void Init(IDalamudPluginInterface pluginInterface);
}