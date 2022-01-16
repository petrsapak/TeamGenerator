using System.Collections.Generic;
using System.Text.Json;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure
{
    public class PlayerDataService : IPlayerDataService
    {
        public string SerializeData(List<Player> data)
        {
            return JsonSerializer.Serialize(data);
        }

        public List<Player> DeserializeData(string serializedData)
        {
            return JsonSerializer.Deserialize<List<Player>>(serializedData);
        }
    }
}
