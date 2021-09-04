using System.Collections.Generic;
using System.Text.Json;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure
{
    public class PlayerDataManager
    {
        public string SerializePlayerPool(List<Player> playerPool)
        {
            string serializedPlayerPool = JsonSerializer.Serialize(playerPool);
            return serializedPlayerPool;
        }
    }
}
