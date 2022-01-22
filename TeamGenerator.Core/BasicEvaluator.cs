using System.Collections.Generic;
using System.Linq;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core
{
    public class BasicEvaluator : IEvaluate
    {
        public double EvaluatePlayer(Player player)
        {
            return player.Rank.Value;
        }

        public double EvaluateTeam(Team team)
        {
            return team.Players.Sum(player => player.Rank.Value);
        }

        public double EvaluateTeamWithoutBots(Team team)
        {
            IEnumerable<Player> realPlayers = team.Players.Where(player => player.Bot == false);
            return realPlayers.Sum(player => player.Rank.Value);
        }
    }
}
