using Dalamud.Plugin;

namespace PartyFiltering.Services;

public interface IIinitializable
{
    void Init(IDalamudPluginInterface pluginInterface);
}