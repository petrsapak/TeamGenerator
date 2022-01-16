namespace TeamGenerator.Infrastructure
{
    public interface IDataService<T>
    {
        string SerializeData(T data);
        T DeserializeData(string serializedData);
    }
}