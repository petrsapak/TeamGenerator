using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Infrastructure
{
    public interface IPlayerDataService
    {
        string SerializeData(List<Player> playerPool);
        List<Player> DeserializeData(string serializedPlayerPool);
    }
}
