namespace PartyFiltering.Serializables.SerializationRepository;

public interface ISerializationRepository
{
    public T? Deserialize<T>(string path);
    public string Serialize<T>(T obj);
}