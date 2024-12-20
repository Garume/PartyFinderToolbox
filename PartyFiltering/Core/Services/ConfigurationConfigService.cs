using PartyFiltering.Core.Serializables;
using PartyFiltering.Shared.Services;

namespace PartyFiltering.Core.Services;

[LoadService(Priority = 500)]
public class ConfigurationConfigService : ConfigService<Configuration>
{
    protected override string ConfigFile => "config.json";
}