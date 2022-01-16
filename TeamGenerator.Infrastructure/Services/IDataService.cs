namespace TeamGenerator.Infrastructure.Services
{
    public interface IDataService<T>
    {
        string SerializeData(T data);
        T DeserializeData(string serializedData);
    }
}