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
            double rankSum = team.Players.Values.Sum(player => player.Rank.Value);
            return rankSum;
        }

        public double EvaluateTeamWithoutBots(Team team)
        {
            IEnumerable<Player> realPlayers = team.Players.Values.Where(player => player.Bot == false);
            double rankSum = realPlayers.Sum(player => player.Rank.Value);
            return rankSum;
        }
    }
}
