using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core
{
    public class BasicEvaluator : IEvaluate
    {
        public int EvaluatePlayer(Player player)
        {
            //enum enumeration starts at 0
            return (int)player.Rank + 1;
        }

        public int EvaluateTeam(Team team)
        {
            int rankCounter = 0;

            foreach (Player player in team.Players.Values)
            {
                rankCounter += EvaluatePlayer(player);
            }

            return rankCounter;
        }
    }
}
