using System.Collections.Generic;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Interfaces
{
    public interface IGenerate
    {
        (Team, Team) GenerateTeams(IEnumerable<Player> availablePlayers);
    }
}
