using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure
{
    public interface IPlayerDataService
    {
        string SerializePlayerPool(List<Player> playerPool);
        List<Player> DeserializePlayerPool(string serializedPlayerPool);
    }
}
