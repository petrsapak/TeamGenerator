using System;
using TeamGenerator.Model;

namespace TeamGenerator.Core.Tests
{
    public class EvaluatorTestHelper
    {
        public static Team GenerateRandomTeam()
        {
            Random random = new Random();
            Team team = new Team("Test");
            CSGORanks ranks = new CSGORanks();

            int teamSize = random.Next(3, 9);

            for (int i = 0; i < teamSize; i++)
            {
                string name = $"Player{i}";
                Rank rank = ranks.Ranks[random.Next(ranks.Ranks.Count)];
                Player player = new Player(name, rank);

                team.AddPlayer(player);
            }

            return team;
        }

        public static double GetSumOfIndividualEvaluations(Team team)
        {
            double rankCounter = 0;

            foreach (Player player in team.Players.Values)
            {
                rankCounter += player.Rank.Value;
            }

            return rankCounter;
        }
    }
}
