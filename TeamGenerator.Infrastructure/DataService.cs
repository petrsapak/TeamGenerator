using System.Text.Json;

namespace TeamGenerator.Infrastructure
{
    public class DataService<T> : IDataService<T>
    {
        public string SerializeData(T data)
        {
            return JsonSerializer.Serialize(data);
        }

        public T DeserializeData(string serializedData)
        {
            return JsonSerializer.Deserialize<T>(serializedData);
        }
    }
}
