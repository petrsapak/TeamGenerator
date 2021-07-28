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
            double rankCounter = 0;

            foreach (Player player in team.Players.Values)
            {
                rankCounter += EvaluatePlayer(player);
            }

            return rankCounter;
        }
    }
}
