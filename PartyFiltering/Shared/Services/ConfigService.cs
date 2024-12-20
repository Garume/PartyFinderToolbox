using System.Text;
using PartyFiltering.Shared.SerializationRepository;

namespace PartyFiltering.Shared.Services;

public abstract class ConfigService<T> : Service<ConfigService<T>> where T : class
{
    private static string _pluginConfigDirectoryPath;
    private static string _configFileName;
    private FileInfo? _configFile;

    public static T? Config { get; private set; }

    public static ISerializationRepository SerializationRepository { get; set; } = new SystemSerializationRepository();

    protected abstract string ConfigFile { get; }

    protected override void Initialize()
    {
        _pluginConfigDirectoryPath = PluginInterface.GetPluginConfigDirectory();
        _configFile = PluginInterface.ConfigFile;

        _configFileName = ConfigFile;
        Migrate(_configFileName);
        Config = Load(_configFileName);
    }

    private T Load(string fileName, bool isFullPath = false)
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
            Config = null;
            File.Move(_configFile.FullName, $"{_configFile}.old");
        }
    }

    public static void Save()
    {
        if (string.IsNullOrEmpty(_configFileName)) return;
        Save(_configFileName);
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