using System.Text.Json;

namespace PartyFiltering.Shared.SerializationRepository;

public class SystemSerializationRepository : ISerializationRepository
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public T? Deserialize<T>(string path)
    {
        return JsonSerializer.Deserialize<T>(path, _options);
    }

    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, _options);
    }
}