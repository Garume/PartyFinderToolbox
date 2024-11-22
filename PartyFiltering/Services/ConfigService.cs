using System.Text;
using Dalamud.Plugin;
using PartyFiltering.Serializables.SerializationRepository;

namespace PartyFiltering.Services;

public class ConfigService<T> : IIinitializable
{
    private static string _pluginConfigDirectoryPath;
    private FileInfo _configFile;

    public static T? Config { get; private set; }
    public static ISerializationRepository SerializationRepository { get; set; } = new SystemSerializationRepository();

    public void Init(IDalamudPluginInterface pluginInterface)
    {
        _pluginConfigDirectoryPath = pluginInterface.GetPluginConfigDirectory();
        _configFile = pluginInterface.ConfigFile;


        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            Logger.Warning($"Loaded assembly: {assembly.GetName().FullName}");

        Migrate("config.json");
        Config = Load("config.json");
    }

    private T? Load(string fileName, bool isFullPath = false)
    {
        var path = fileName;
        if (!isFullPath) path = Path.Combine(_pluginConfigDirectoryPath, fileName);
        if (!File.Exists(path)) return Activator.CreateInstance<T>();
        var content = File.ReadAllText(path, Encoding.UTF8);
        return SerializationRepository.Deserialize<T>(content) ?? Activator.CreateInstance<T>();
    }

    private void Migrate(string fileName)
    {
        var path = Path.Combine(_pluginConfigDirectoryPath, fileName);
        if (!File.Exists(path) && _configFile.Exists)
        {
            Logger.Warning($"Migrating config file from {_configFile.FullName} to {path}");
            Config = Load(_configFile.FullName, true);
            Save(fileName);
            Config = default;
            File.Move(_configFile.FullName, $"{_configFile}.old");
        }
    }

    public static void Save(string fileName)
    {
        if (Config == null) return;
        var serialized = SerializationRepository.Serialize(Config);

        try
        {
            var path = Path.Combine(_pluginConfigDirectoryPath, fileName);
            File.WriteAllText(path, serialized, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
}