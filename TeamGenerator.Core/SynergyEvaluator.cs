using System.Collections.Generic;
using System.Linq;
using TeamGenerator.Core.Interfaces;
using TeamGenerator.Model;

namespace TeamGenerator.Core
{
    public class SynergyEvaluator : IEvaluate
    {
        public double EvaluatePlayer(Player player)
        {
            return player.Rank.Value;
        }

        public double EvaluateTeam(Team team)
        {
            double teamEvaluation = 0;

            foreach (Player player in team.Players)
            {
                teamEvaluation += player.Rank.Value;
                foreach (Player player2 in team.Players)
                {
                    if (player2 != player && player2.Rank.Value >= 10)
                    {
                        if (player2.Rank.Value >= (player.Rank.Value + 2))
                        {
                            teamEvaluation += 2;
                        }

                        else if (player2.Rank.Value >= (player.Rank.Value + 3))
                        {
                            teamEvaluation += 3;
                        }
                    }
                }
            }

            return teamEvaluation;
        }

        public double EvaluateTeamWithoutBots(Team team)
        {
            IEnumerable<Player> realPlayers = team.Players.Where(player => player.Bot == false);
            return realPlayers.Sum(player => player.Rank.Value);
        }
    }
}
