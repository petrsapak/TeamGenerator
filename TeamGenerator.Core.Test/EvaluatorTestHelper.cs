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
            Array rankValues = Enum.GetValues(typeof(Rank));

            int teamSize = random.Next(3, 9);

            for (int i = 0; i < teamSize; i++)
            {
                string name = $"Player{i}";
                Rank rank = (Rank)rankValues.GetValue(random.Next(rankValues.Length));
                Player player = new Player(name, rank);

                team.AddPlayer(player);
            }

            return team;
        }

        public static int GetSumOfIndividualEvaluations(Team team)
        {
            int rankCounter = 0;

            foreach (Player player in team.Players.Values)
            {
                //enum enumeration starts at 0
                rankCounter += (int)player.Rank + 1;
            }

            return rankCounter;
        }
    }
}
