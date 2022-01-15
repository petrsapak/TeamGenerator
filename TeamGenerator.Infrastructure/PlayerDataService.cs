using System.Collections.Generic;
using System.Text.Json;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure
{
    public class PlayerDataService : IPlayerDataService
    {
        public string SerializePlayerPool(List<Player> playerPool)
        {
            string serializedPlayerPool = JsonSerializer.Serialize(playerPool);
            return serializedPlayerPool;
        }

        public List<Player> DeserializePlayerPool(string serializedPlayerPool)
        {
            List<Player> playerPool = JsonSerializer.Deserialize<List<Player>>(serializedPlayerPool);
            return playerPool;
        }
    }
}
