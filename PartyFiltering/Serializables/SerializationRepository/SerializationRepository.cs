using Newtonsoft.Json;

namespace PartyFiltering.Serializables.SerializationRepository;

public class SerializationRepository : ISerializationRepository
{
    private readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
        Formatting = Formatting.Indented,
        ObjectCreationHandling = ObjectCreationHandling.Replace
    };

    public T? Deserialize<T>(string path)
    {
        return JsonConvert.DeserializeObject<T>(path, _settings);
    }

    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, _settings);
    }
}