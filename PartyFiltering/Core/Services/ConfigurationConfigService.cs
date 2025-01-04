using PartyFinderToolbox.Core.Serializables;
using PartyFinderToolbox.Shared.Services;

namespace PartyFinderToolbox.Core.Services;

[LoadService(Priority = 500)]
public class ConfigurationConfigService : ConfigService<Configuration>
{
    protected override string ConfigFile => "config.json";
}